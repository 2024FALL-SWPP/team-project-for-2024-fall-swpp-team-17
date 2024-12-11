# Team17 Gravitable Escape🧑‍🚀

## 1. 전체적인 작업 방법

1. **Main 브랜치에서 직접 작업하지 않고** 각자 브랜치를 생성하여 작업합니다. 브랜치명은 아래 **Branch Naming Convention**을 따릅니다.
2. 각자 브랜치에서 작업을 완료할 때마다 **commit을 자주 하고**(블렌더 작업도 포함), 커밋 메시지는 아래 **Commit Message Convention**을 따릅니다.
3. 최종 작업물은 본인의 브랜치에 push 한 후 <b>main 브랜치와 병합(Merge)</b>하기 위해 **pull request**를 생성합니다.
4. GitHub 설정상 main 브랜치는 바로 수정할 수 없고, 브랜치를 생성하여 pull request를 통해서만 병합할 수 있습니다.
5. Pull request는 작성자 외 다른 팀원 1명의 승인을 받아야 main 브랜치로 병합됩니다. PM님 또는 팀원이 리뷰 후 승인 또는 개선 요청을 남깁니다.  
6. Pull request가 생성되면 **Slack에 알림을 남기고, 팀원들이 수시로 확인하여 리뷰**할 수 있도록 합니다.

## 2. Naming Convention

### 2.1 Branch Naming Convention

- **모두 소문자** 사용
- <b>띄어쓰기는 하이픈(-)</b>으로 구분
- **[이름]/[대분류]/[소분류]/[설명]** 형식으로 작성

예시:
- `minjung/unity/feat/wormhole-player-interaction`
- `minjung/unity/fix/camera-mouse-movement`
- `suzy/blender/design/background`

**대분류:**
- `unity` - Unity 관련 작업
- `blender` - Blender 관련 작업
- `etc` - 기타 작업

**소분류:**
- `feat/` : 새로운 기능 추가 (feature)
- `fix/` : 버그 수정 (bugfix)
- `design/` : Blender 에셋 작업
- `chore/` : 코드 정리, 주석 추가, 환경 설정 (maintenance tasks)
- `test/` : 테스트 관련 작업
- `doc/` : 문서화 작업

### 2.2 Commit Message Convention

- **소분류는 소문자**로, 설명은 **대문자로 시작**합니다.
- **[소분류]: [설명]** 형식으로 작성하며, 소분류는 위의 Branch 소분류와 동일하게 사용합니다.

예시:
- `feat: Add camera mouse movement`
- `fix: Fixed player rotation when gravity changes`

## 3. 작업 순서 요약

1. 새 브랜치 생성: `git checkout -b [브랜치 이름]`
2. 작업 후 변경 사항 커밋: `git add .` → `git commit -m "[커밋 메시지]"`
3. 원격 저장소에 Push: `git push -u origin [브랜치 이름]`
4. Pull request 생성 후 리뷰 요청
5. 리뷰 승인 시 main 브랜치로 병합

---
