using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    public GameObject[] Manekins;
    public Material[] materials;
    public GameObject render_Image;
    RenderImage renderImage;
    MeshRenderer[] meshRenderers;
    public Transform barTrans_Parent;
    Transform barTrans;

    private void Awake()
    {
        Manekins = new GameObject[4];
        meshRenderers = new MeshRenderer[2];
        for(int i = 0; i < 4; i++)
        {
            Manekins[i] = transform.GetChild(0).GetChild(i).gameObject;
            
        }
        renderImage = render_Image.GetComponent<RenderImage>();
        meshRenderers[0] = transform.GetChild(1).GetComponent<MeshRenderer>();
        meshRenderers[1] = transform.GetChild(2).GetComponent<MeshRenderer>();
        barTrans = barTrans_Parent.GetChild(4);
    }

    public void ChangeCharacter(int index)
    {
        EquipManager.Instance.character = (CharacterItemSprite)index;
        renderImage.touched = false;
        for (int i = 0; i < Manekins.Length; i++)
        {
            if (i == index)
            {
                barTrans.position = barTrans_Parent.GetChild(i).position;
                Manekins[i].gameObject.SetActive(true);
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    meshRenderer.material = materials[i];
                }                
            }
            else
            {
                Manekins[i].gameObject.SetActive(false);
            }
        }
        
    }

}
