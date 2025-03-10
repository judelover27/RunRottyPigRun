using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Unity 에디터에서 실행 중이면 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        // WebGL에서는 게임 종료 불가 (알림 표시)
        Debug.Log("WebGL에서는 Application.Quit()을 사용할 수 없습니다!");
#else
        // 그 외 플랫폼에서는 정상 종료
        Application.Quit();
#endif
    }
}
