using UnityEngine;

public class InputManager : MonoBehaviour
{
    private CharacterChoiceUI characterChoiceUI; // Inspector 연결 대신 직접 찾음
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SkillPressed { get; private set; }
    public bool CharacterChanged { get; private set; }
    public bool AttackPressed { get; private set; }

    private bool isSelecting = false;
    private void Awake()
    {
        // 자동 생성된 오브젝트에서 CharacterChoiceUI 찾아서 연결
        characterChoiceUI = FindObjectOfType<CharacterChoiceUI>();

        if (characterChoiceUI == null)
            Debug.LogError("CharacterChoiceUI를 찾을 수 없습니다. 씬에 존재해야 합니다.");
    }
    private void Update()
    {
        bool inCharacterSelect = Input.GetKey(KeyCode.LeftAlt);

        // Alt 키를 처음 눌렀을 때
        if (inCharacterSelect && !isSelecting)
        {
            isSelecting = true;
            characterChoiceUI.PauseTime(); // 슬로우 대신 완전 정지
            int currentIndex = Managers.Character.CurrentIndex; // 현재 캐릭터 인덱스 가져오기
            characterChoiceUI.Show(currentIndex); // 현재 캐릭터부터 선택 시작
        }

        // Alt 키를 떼면 선택 확정
        if (!inCharacterSelect && isSelecting)
        {
            isSelecting = false;
            characterChoiceUI.ResumeTime(); // 재개
            int selected = characterChoiceUI.GetSelectedIndex();
            Managers.Character.ChangeCharacter(selected);
            CharacterChanged = true;
            characterChoiceUI.Hide();
        }

        // 이동 입력 (선택 중엔 비활성)
        if (!inCharacterSelect)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if (Managers.Gravity.GetGravityState())
                x *= -1;
            MoveInput = new Vector2(x, y).normalized;

            JumpPressed = Input.GetKeyDown(KeyCode.UpArrow);
            SkillPressed = Input.GetKeyDown(KeyCode.Space);
        }
        else
        {
            MoveInput = Vector2.zero;
        }

        // 선택 중일 때 좌/우 방향키로 선택 이동
        if (isSelecting)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                characterChoiceUI.Move(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                characterChoiceUI.Move(1);
        }

        // 기본 공격 입력 (선택 중엔 무시)
        AttackPressed = !inCharacterSelect && Input.GetKeyDown(KeyCode.F);
    }

    public void ClearInputs()
    {
        JumpPressed = false;
        SkillPressed = false;
        AttackPressed = false;
    }
}
