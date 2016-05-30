using UnityEngine;

/** Detail view is accomplished by doing the following:
 *  - To GROW (i.e. show) detail view:
 *    - Create a new GameObject with the same material as this GameObject.
 *    - Set its destination to the cameras position.
 *    - Move it using Vector3.Lerp in Update()
 *  - To SHRINK (i.e. cancel) detail view:
 *    - Set destination to the original position of the item.
 *    - Move it using vector3.Lerp in Update()
 *    
 *  While growing or shrinking, mouse movement is disabled.
 *  TODO: Hide mouse cursor while growing/shrinking.
 *  
 *  TODO: There is slight flickering on detail view activation,
 *  presumably from the object being created. Consider setting the
 *  MeshRenderer of the prefab to disabled by default, and then enable it
 *  once the material has been set. It might also be because the two items
 *  overlap for an instance, in which case move it slightly above the original
 *  item.
 */
public class DetailViewMode : MonoBehaviour {
    public float growAnimationTime = 3f;
    public float shrinkAnimationTime = 1.5f;
    public float scaleMultiplier = 6f;
    private Transform dataItem;

    private Vector3 originalPos;

    private Vector3 target;
    private bool isGrown = false;
    private bool isTransforming = false;
    private float startTime;
    private float journeyLength;
    public float speed = 1F;
    public GameObject prefab;
    private GameObject player;
    private ExperimentControl experimentCtrl;
    private GameObject detailView;

    void Awake()
    {
        this.dataItem = transform.GetChild(0);
        this.player = GameObject.Find("Player");
        this.experimentCtrl = this.player.GetComponent<ExperimentControl>();
    }

    private void CreateDetailView()
    {
        GameObject spawned = (GameObject)Instantiate(this.prefab, this.transform.position, this.transform.rotation);
        spawned.name = this.gameObject.name + " DetailView";
        spawned.transform.SetParent(this.transform);
        spawned.GetComponentInChildren<MeshRenderer>().material = this.dataItem.GetComponent<MeshRenderer>().material;
        this.detailView = spawned;
    }

    public void GrowOrShrink()
    {
        if (isTransforming) return;

        this.isTransforming = true;
        this.player.GetComponent<Movement>().enabled = false;
        this.player.GetComponentInChildren<PlayerCollision>().enabled = false;

        if (isGrown)
        {
            this.target = this.originalPos;
        } else
        {
            if(this.detailView != null)
            {
                GameObject.Destroy(this.detailView);
                this.detailView = null;
            }

            CreateDetailView();
            Vector3 centerPoint = Camera.main.ScreenToWorldPoint(
                new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
            Vector3 A = this.detailView.transform.position;
            Vector3 B = centerPoint;
            Vector3 midway = A + (B - A) * 0.90f;
            this.target = midway;
            this.originalPos = this.detailView.transform.position;
        }
        
        this.startTime = Time.time;
        this.journeyLength = Vector3.Distance(this.detailView.transform.position, this.target);
    }

    void Update()
    {
        if(this.isTransforming)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            this.detailView.transform.position = Vector3.Lerp(this.detailView.transform.position, this.target, fracJourney);
            if (Vector3.Distance(this.detailView.transform.position, this.target) < 0.1f)
            {
                this.isTransforming = false;
                this.isGrown = !this.isGrown;

                if(!this.isGrown)
                {
                    GameObject.Destroy(this.detailView);
                    this.player.GetComponent<Movement>().enabled = true;
                    this.player.GetComponentInChildren<PlayerCollision>().enabled = true;
                }
            }
        }
    }
}
