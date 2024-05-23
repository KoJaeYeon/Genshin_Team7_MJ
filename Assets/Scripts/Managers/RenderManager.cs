using UnityEngine;
using System;

public class RenderManager : MonoBehaviour
{
    public GameObject[] Manekins;
    public Material[] materials;
    public Material[] materialstexture;
    public GameObject render_Image;
    RenderImage renderImage;
    MeshRenderer[] meshRenderers;
    public Transform barTrans_Parent;
    Transform barTrans;
    public PropertyText_Weapon weaponPropertyText;
    public PropertyText_Relic relicPropertyText;
    public ParticleSystem particle;
    public ParticleSystem smoke;

    private void Awake()
    {
        Manekins = new GameObject[4]; // ����ŷ ĳ���� �迭 �ʱ�ȭ
        meshRenderers = new MeshRenderer[3]; // ���͸��� �ٲ�� �ϴ� �Ž� ������ �ʱ�ȭ 0 �����, 1 ������Ʈ, 2, �ٴ�
        for (int i = 0; i < 4; i++)
        {
            Manekins[i] = transform.GetChild(0).GetChild(i).gameObject; // ����ŷ �޾ƿ���
            
        }

        //������Ʈ �޾ƿ���
        renderImage = render_Image.GetComponent<RenderImage>();

        meshRenderers[0] = transform.GetChild(2).GetComponent<MeshRenderer>();

        meshRenderers[1] = transform.GetChild(3).GetComponent<MeshRenderer>();
        meshRenderers[2] = transform.GetChild (4).GetComponent<MeshRenderer>();

        barTrans = barTrans_Parent.GetChild(4);
    }

    public void ChangeCharacter(int index) // ĳ���� �гο��� ĳ���� ������ ĳ���� ���� 
    {
        EquipManager.Instance.character = (CharacterItemSprite)index; //��ȯ�Ǿ�� �ϴ� ĳ���� ����
        weaponPropertyText.UpdatePanel(); // ���â �г� �ش� ĳ���ͷ� ������Ʈ
        relicPropertyText.UpdatePanel(); // ������â ������Ʈ
        renderImage.touched = false; // ĳ���� �ٲ� ī�޶� �ʱ�ȭ

        for (int i = 0; i < Manekins.Length; i++)
        {
            if (i == index) // �ش� ĳ�����϶� Ȱ��ȭ�� ��� ����
            {
              
                    barTrans.position = barTrans_Parent.GetChild(i).position;
                    Manekins[i].gameObject.SetActive(true);

                    meshRenderers[0].material = materials[i];
                    meshRenderers[1].material = materialstexture[i];
                    meshRenderers[2].material = materialstexture[i];

                    //��ƼŬ
                    var colorOverLifetime = particle.colorOverLifetime;
                    ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient(materials[i].color);
                    colorOverLifetime.color = gradient;
            

            }
            else // �ƴϸ� ĳ���� ��Ȱ��ȭ
            {
                Manekins[i].gameObject.SetActive(false);
            }
        }
        
    }

    

}
