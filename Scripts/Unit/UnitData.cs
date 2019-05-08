using UnityEngine;

[System.Serializable]
public class UnitData
{
	/* 애니메이션 */
	public AnimationImage[] _animImages; // 이미지
	[HideInInspector] public bool _isAnimPlaying; // 애니메이션이 재생중인가
	[HideInInspector] public int _animIndex; // 애니메이션 이미지 인덱스

	/* 속도값들 */
	[Space ( 20 )]
	public float _walkSpeed; // 걷는 속도
	public float _runSpeed; // 뛰는 속도
	public float _curSpeed; // 현재 속도

	/* 상태값들 */
	[HideInInspector] public bool _isIdle; // 가만히있는 상태
	[HideInInspector] public bool _isWalking; // 걷고있는 상태
	[HideInInspector] public bool _isRunning; // 뛰고있는 상태

	/* 방향 */
	[HideInInspector] public Vector2 _direction; // 위(0, 1), 아래(0, -1), 왼쪽(-1, 0), 오른쪽(1, 0)

	public void NextAnimIndex ()
	{
		_animIndex++;

		if ( _direction == new Vector2 ( 0, 1 ) ) _animIndex %= _animImages[0]._walk.Length;
		else if ( _direction == new Vector2 ( 0, -1 ) ) _animIndex %= _animImages[1]._walk.Length;
		else if ( _direction == new Vector2 ( -1, 0 ) ) _animIndex %= _animImages[2]._walk.Length;
		else if ( _direction == new Vector2 ( 1, 0 ) ) _animIndex %= _animImages[3]._walk.Length;
	}
}

[System.Serializable]
public class AnimationImage
{
	public Sprite _idle;
	public Sprite[] _walk; // 0:위, 1:아래, 2:왼쪽, 3:오른쪽
}
