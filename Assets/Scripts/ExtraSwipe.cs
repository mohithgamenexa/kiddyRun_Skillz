
using UnityEngine;

public class ExtraSwipe : MonoBehaviour
{
    Transform mTransform;
    Animator anim;
    MainCharacter chr;
    void Awake()
    {
        mTransform = transform;
        chr = GetComponent<MainCharacter>();
        anim = GetComponent<Animator>();
    }
    bool inFrame = false;
    void Update()
    {
        float distance = Mathf.Abs(50 - mTransform.position.x);
        if(distance < 2f)
        {
            Vector3 pos = mTransform.localPosition;
            pos.z = 4.5f + distance*2f;
            mTransform.localPosition = pos;
            if (!inFrame)
            {
                inFrame = true;
                anim.SetBool("inframe", true);
            }
     
        }else if (inFrame)
        {
            inFrame = false;
            anim.SetBool("inframe", false);
        }
    }
    void OnFinished()
    {
        if(chr.character == CharacterType.Boxer)
        {
            anim.SetInteger("idno", 1);
        }
    }
   
}
