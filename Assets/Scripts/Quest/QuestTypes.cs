using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest
{
    class QuestEatDango : QuestData
    {
        int _specifyCount;          //食べた数
        int _eatCount;              //食べる数
        int _continueCount;         //連続して作る数
        int _currentContinueCount;  //連続して作った数

        bool _canCountCreateRole;   //役を作ったときにカウントするか
        bool _canCountNoCreateRole; //作らなかったときにカウントするか

        bool _isPrebCreateRole;     //直前に役を作って食べたか

        List<DangoColor> _colors = new(); //この色だけ読み取る

        public QuestEatDango(int id, int specifyCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
            : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _specifyCount = specifyCount;
            _continueCount = continueCount;
            _canCountCreateRole = canCountCreateRole;
            _canCountNoCreateRole = canCountNoCreateRole;

            for (DangoColor i = DangoColor.None + 1; i < DangoColor.Other; i++)
                _colors.Add(i);
        }
        public QuestEatDango(int id, List<DangoColor> colors, int specifyCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
            : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _specifyCount = specifyCount;
            _continueCount = continueCount;
            _canCountCreateRole = canCountCreateRole;
            _canCountNoCreateRole = canCountNoCreateRole;

            _colors = colors;
        }
        public QuestEatDango(int id, DangoColor[] colors, int specifyCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
            : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _specifyCount = specifyCount;
            _continueCount = continueCount;
            _canCountCreateRole = canCountCreateRole;
            _canCountNoCreateRole = canCountNoCreateRole;

            _colors.AddRange(colors);
        }
        public QuestEatDango(int id, DangoColor color, int specifyCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
            : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _specifyCount = specifyCount;
            _continueCount = continueCount;
            _canCountCreateRole = canCountCreateRole;
            _canCountNoCreateRole = canCountNoCreateRole;

            _colors.Add(color);
        }

        public int SpecifyCount => _specifyCount;
        public int EatCount => _eatCount;
        public int ContinueCount => _continueCount;
        public int CurrentContinueCount => _currentContinueCount;
        public bool CanCountCreateRole => _canCountCreateRole;
        public bool CanCountNoCreateRole => _canCountNoCreateRole;
        public bool IsPrebCreateRole => _isPrebCreateRole;
        public List<DangoColor> Colors => _colors;
        public void AddEatCount() => _eatCount++;
        public void AddEatCount(int count) => _eatCount += count;
        public void AddContinueCount() => _currentContinueCount++;
        public void ResetContinueCount() => _currentContinueCount = 0;
        public void SetIsPrebCreateRole(bool isPrebCreateRole) => _isPrebCreateRole = isPrebCreateRole;
    }

    class QuestCreateRole : QuestData
    {
        string _roleName;
        int _specifyCount;  //作る数
        int _madeCount;     //作った数
        int _continueCount; //連続して作る数
        int _currentContinueCount; //連続して作った数

        bool _createRole;   //役を「作ったとき」か「作らなかったとき」か
        bool _enableAnyRole;//すべての役を許可するか
        bool _onlyPerfectRole; //完全役（D5が現在のマックスのときだけ）のみ
        int _colorCount;    //色の数

        bool _isPrebCreateRole;     //直前に役を作って食べたか

        List<DangoColor> _colors = new(); //この色だけ読み取る

        public QuestCreateRole(int id, string roleName, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _roleName = roleName;
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = false;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors.Add(color);
        }
        public QuestCreateRole(int id, string roleName, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _roleName = roleName;
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = false;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors.Add(color);
        }
        public QuestCreateRole(int id, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = true;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors.Add(color);
        }
        public QuestCreateRole(int id, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = true;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors.Add(color);
        }
        public QuestCreateRole(int id, string roleName, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _roleName = roleName;
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = false;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors.AddRange(colors);
        }
        public QuestCreateRole(int id, string roleName, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _roleName = roleName;
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = false;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors.AddRange(colors);
        }
        public QuestCreateRole(int id, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = true;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors.AddRange(colors);
        }
        public QuestCreateRole(int id, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = true;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors.AddRange(colors);
        }
        public QuestCreateRole(int id, Role<int> role, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _roleName = role.GetRolename();
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = false;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors = colors;
        }
        public QuestCreateRole(int id, Role<int> role, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _roleName = role.GetRolename();
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = false;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors = colors;
        }
        public QuestCreateRole(int id, string roleName, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _roleName = roleName;
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = false;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors = colors;
        }
        public QuestCreateRole(int id, string roleName, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _roleName = roleName;
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = false;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors = colors;
        }
        public QuestCreateRole(int id, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = true;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors = colors;
        }
        public QuestCreateRole(int id, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _onlyPerfectRole = onlyPerfectRole;
            _specifyCount = specifyCount;
            _enableAnyRole = true;
            _createRole = createRole;
            _continueCount = continueCount;
            _colorCount = colorCount;
            _colors = colors;
        }

        public string RoleName => _roleName;
        public bool OnlyPerfectRole => _onlyPerfectRole;
        public int SpecifyCount => _specifyCount;
        public int MadeCount => _madeCount;
        public int ContinueCount => _continueCount;
        public int CurrentContinueCount => _currentContinueCount;
        public bool CreateRole => _createRole;
        public bool EnableAnyRole => _enableAnyRole;
        public int ColorCount => _colorCount;
        public bool IsPrebCreateRole => _isPrebCreateRole;
        public List<DangoColor> Colors => _colors;
        public void AddMadeCount() => _madeCount++;
        public void AddContinueCount() => _currentContinueCount++;
        public void ResetContinueCount() => _currentContinueCount = 0;
        public void SetIsPrebCreateRole(bool isPrebCreateRole) => _isPrebCreateRole = isPrebCreateRole;
    }

    class QuestPlayAction : QuestData
    {
        public enum PlayerAction
        {
            FallAttack,
        }

        PlayerAction _action;
        int _specifyCount;
        int _madeCount;

        public QuestPlayAction(int id, PlayerAction action, int specifyCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.PlayAction, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _action = action;
            _specifyCount = specifyCount;
        }
        public QuestPlayAction(int id, PlayerAction action, int specifyCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId) : base(id, QuestType.PlayAction, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _action = action;
            _specifyCount = specifyCount;
        }
        public QuestPlayAction(int id, PlayerAction action, int specifyCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId) : base(id, QuestType.PlayAction, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _action = action;
            _specifyCount = specifyCount;
        }

        public PlayerAction Action => _action;
        public int SpecifyCount => _specifyCount;
        public int MadeCount => _madeCount;
        public void AddMadeCount() => _madeCount++;
    }

    class QuestDestination : QuestData
    {
        bool _onEatSucceed;

        public QuestDestination(int id, bool onEatSucceed, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId) : base(id, QuestType.Destination, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId)
        {
            _onEatSucceed = onEatSucceed;
        }

        public bool OnEatSucceed => _onEatSucceed;
    }
}