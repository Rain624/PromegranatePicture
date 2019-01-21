using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Qdisa;

public class GameManager : MonoBehaviour
{
    public PostControl postControl;
    public AnimatorControl animatorControl;
    public QdisaStateMachine GameStateMachine;
    private QdisaState manState;
    private QdisaState unmanState;
    private QdisaTransition man2unman;
    private QdisaTransition unman2man;

    private bool isMan;

    // Start is called before the first frame update
    void Start()
    {
        InitFsm();
        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.Space))
            .Subscribe(_ =>
            {
                isMan = true;
            });

    }
    private void Update()
    {
        GameStateMachine.UpdateCallback(Time.deltaTime);
    }
    private void InitFsm()
    {
        manState = new QdisaState("Man");
        manState.OnEnter += (IState state) =>
          {
              animatorControl.PlayAni();
              animatorControl.OnAniComplete += postControl.BloomScreen;
              animatorControl.OnAniComplete += SetUnman;
          };
        manState.OnExit += (IState state) =>
          {
              //animatorControl.OnAniComplete -= postControl.BloomScreen;
              
          };
        unmanState = new QdisaState("UnMan");
        unmanState.OnEnter += (IState state) =>
          {
              Debug.Log("我在无人状态");
          };
        man2unman = new QdisaTransition("Man2Unman", manState, unmanState);
        man2unman.OnCheck += () =>
        {
            return !isMan;
        };
        manState.AddTransition(man2unman);
        unman2man = new QdisaTransition("Unman2Man", unmanState, manState);
        unman2man.OnCheck += () =>
        {
            return isMan;
        };
        unmanState.AddTransition(unman2man);

        GameStateMachine = new QdisaStateMachine("Root", unmanState);
       
    }
    private void SetUnman()
    {
        if(!Timer.Exist("toUnman"))
        {
            Timer.AddTimer(5, "toUnman")
                .OnCompleted(() =>
                {
                    isMan = false;
                }
                );
        }

    }
}
