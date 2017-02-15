using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour
{

    public float radius = 5.0F;
    public float power = 10.0F;
    public Player owner;

    public float created, dieAt, LifeSpan;

    void Awake()
    {
        created = Time.time;
        dieAt = created + LifeSpan;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Time.time >= dieAt)
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(power * 2, explosionPos, radius, power, ForceMode.Impulse);
                }

     
                    mapTile disTile = hit.GetComponent<mapTile>();

                if (disTile != null)
                {

                    if (disTile.disType == mapTile.TileType.floor)
                    {
                        if (disTile.initialTilePos.y > 0)
                        {
                            Debug.Log("bomb hit grass");

                            owner.stageGen.DestroyTile(disTile);
                        }

                    }
                    else if (disTile.disType == mapTile.TileType.obstacle)
                    {
                        if (disTile.initialTilePos.y > 0)
                        {
                            Debug.Log("bomb hit obstacle");

                            owner.stageGen.DestroyTile(disTile);
                        }

                    }
                    else if (disTile.disType == mapTile.TileType.enemy)
                    {

                        owner.kills++;
                        owner.stageGen.enesCount--;
                        Debug.Log("bomb hit ene");

                        owner.stageGen.DestroyTile(disTile);

                    }
                    else if (disTile.disType == mapTile.TileType.border)
                    {

                        Debug.Log("bomb hit border");

                    }

                }
            }

            Destroy(this.gameObject);
        }


    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }

}
