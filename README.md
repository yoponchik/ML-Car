![image](https://user-images.githubusercontent.com/57009810/147415408-89955cda-2519-4bb0-a51b-ca1d8fae1ddd.png)
# ML-Project

Imitation Learning for AI car using rays.

<h2>Project Details</h2>

**Project Date:** 2021.09

**Dev Tools:** Unity Engine, ML Agents



환경설정
---------
AI 자동차가 직진하도록 트랙 곳곳에 체크포인트를 배치

그리고 자동차가 트랙에서 떨어졌다고 인식하는 OffTrack게임오브젝트



![image](https://user-images.githubusercontent.com/57009810/147415077-cafc73ec-7091-412d-91b8-c42fab84df57.png)




Check Points
------------
차 앞에만 Checkpoint 생성해서 후진하지 않게 유도함
배열 두개 할당해서 SetActive 방식으로 활성화/비활성화

트랙 전반부를 도달하면 나머지 체크포인트가 생성됨

![image](https://user-images.githubusercontent.com/57009810/147415096-bac0c814-4057-48be-aa3a-8f54f262d7b8.png)



Imitation Learning
-------------------

레이싱 트랙이 벽이 없어서 강화학습 대신 모방학습을 응용

Heuristic 메서드를 통해서 차가 완주하게 직접 제어하고 Recording

Learning을 실행하면 차가 Ray Perception Sensor 컴포넌트를 활용하여 학습함

Hallway.yaml 샘플 파일 사용




Reward System
----------------
Checkpoint와 충돌하면 +0.2 Reward

OffTrack와 충돌하면 -10 Reward
End Episode

완주하면 +20 Reward
End Episode



결과
---------
Step: 30,770,000
Mean Reward: 20.577 (약 80%?)
![image](https://user-images.githubusercontent.com/57009810/147415126-fab30b9d-126d-4d12-838e-d0be8c9ebf96.png)


개선할 점
--------
(1) Reward System 수정
  (a) Min Reward: -1
  (b) Max Reward: 1
(2) 모방학습한 모델을 강화학습으로 더 개선 

