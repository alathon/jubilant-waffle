using UnityEngine;
using System.Collections;

public class DetailViewMode : MonoBehaviour {
    public float growAnimationTime = 3f;
    public float shrinkAnimationTime = 1.5f;
    public float scaleMultiplier = 6f;
    private Transform dataItem;
    private Camera camera;

    private Vector3 originalPos;
    private Vector3 originalScale;

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
        this.camera = Camera.main;
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
        //this.experimentCtrl.TogglePlayerCollision();

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
            Vector3 centerPoint = camera.ScreenToWorldPoint(
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
                GameObject.Destroy(this.detailView);
                this.detailView = null;
            }
        }
    }
}
