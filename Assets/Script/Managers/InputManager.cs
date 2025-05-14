using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SkillPressed { get; private set; }
    public bool CharacterChanged {  get; private set; }
    public bool AttackPressed { get; private set; }

    private void Update()
    {
        bool inCharacterSelect = Input.GetKey(KeyCode.LeftAlt);

        // 이동 입력
        if (!inCharacterSelect)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            MoveInput = new Vector2(x, y).normalized;
            // 단발성 입력
            JumpPressed = Input.GetKeyDown(KeyCode.UpArrow);
            SkillPressed = Input.GetKeyDown(KeyCode.Space);
        }
        else
        {
            MoveInput = Vector2.zero;
        }


        // 캐릭터 선택
        CharacterChanged = false;

        if (inCharacterSelect)
        { 
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Managers.Character.ChangeCharacter(0);                
                CharacterChanged = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Managers.Character.ChangeCharacter(1);
                CharacterChanged = true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Managers.Character.ChangeCharacter(2);
                CharacterChanged = true;
            }
        }

        //캐릭터 기본 공격
        AttackPressed = Input.GetKeyDown(KeyCode.F);
    }




    // 매 프레임 끝에 단발 입력 플래그 초기화
    public void ClearInputs()
    {
        JumpPressed = false;
        SkillPressed = false;
        AttackPressed = false;
    }
}
