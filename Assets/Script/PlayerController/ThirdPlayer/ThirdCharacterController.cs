using UnityEngine;

public class ThirdCharacterController : BaseCharacterController
{


    protected override void Awake()
    {
        base.Awake(); // BaseCharacterController의 Awake() 호출

    }

    protected override void Update()
    {
        base.Update(); // BaseCharacterController의 Update() 호출       
    }

    //전사 스킬 : 특정 벽 부수기(크리트컬 공격 같은것 데미지 1.5배, 쿨타임 5초)
}