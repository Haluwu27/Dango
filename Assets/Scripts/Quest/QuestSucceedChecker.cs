using System.Collections.Generic;
using Dango.Quest.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Dango.Quest
{
    class QuestSucceedChecker
    {
        QuestManager _manager;
        bool isSucceedThisFrame;

        PlayerUIManager _playerUIManager;

        private async UniTask SetBoolAfterOneFrame(bool enable)
        {
            await UniTask.Yield();

            isSucceedThisFrame = enable;
        }

        public QuestSucceedChecker(QuestManager manager, PlayerUIManager playerUIManager)
        {
            _manager = manager;
            _playerUIManager = playerUIManager;
        }

        #region EatDango
        public bool CheckQuestEatDangoSucceed(QuestManager questManager, List<DangoColor> colors, bool createRole)
        {
            //このフレームに別のクエストがクリアされていたら弾く
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestEatDango questEa)
                {
                    if (CheckQuestSucceed(questEa, colors, createRole)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestEatDango quest, List<DangoColor> colors, bool createRole)
        {
            //役の成立次第で弾くものは弾く
            if (createRole && !quest.AllowCountCreateRole || !createRole && !quest.AllowCountNoCreateRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }

            //色を判定し、正しい色なら食べた数を追加
            foreach (var color in colors)
            {
                if (!quest.ReadColors.Contains(color)) continue;

                quest.AddEatCount();
            }

            if (quest.IsPrebCreateRole != createRole) quest.AddContinueCount();
            else quest.ResetContinueCount();

            //判定の前に今回役を作ったか記録
            quest.SetIsPrebCreateRole(createRole);

            //指定回数作ったか判定
            if (!quest.IsAchievedEatCount()) return false;
            if (!quest.IsAchievedContinueCount()) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region CreateRole
        public bool CheckQuestCreateRoleSucceedEs(List<DangoColor> dangos, bool createRole, int currentMaxDango)
        {
            //このフレームに別のクエストがクリアされていたら弾く
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedEs(questCr, dangos, createRole, currentMaxDango)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedSr(Role<int> role)
        {
            //このフレームに別のクエストがクリアされていたら弾く
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedSr(questCr, role)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedIr(List<DangoColor> dangos)
        {
            //このフレームに別のクエストがクリアされていたら弾く
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedIc(questCr, dangos)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedSm(Role<int> role)
        {
            //このフレームに別のクエストがクリアされていたら弾く
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedSm(questCr, role)) return true;
                }
            }

            return false;
        }

        private bool HasReadColor(QuestCreateRole quest, IEnumerable<DangoColor> dangosDistinct)
        {
            foreach (var color in dangosDistinct)
            {
                //指定色があったら抜ける
                if (quest.Establish.ReadColors.Contains(color)) return true;
            }

            //ぜんぶなかったら弾く
            return false;
        }

        private bool CheckQuestSucceedEs(QuestCreateRole quest, List<DangoColor> dangos, bool createRole, int currentMaxDango)
        {
            //不正なアクセスであれば弾く
            if (quest.CRType != QuestCreateRole.CreateRoleType.EstablishRole) return false;

            //食べた団子がクエスト内容と一致しているか判定
            //役の成立・非成立のフラグが一致していなければ弾く
            if (createRole != quest.Establish.CreateRole)
            {
                quest.ResetContinueCount();
                return false;
            }
            //完全役のみの場合、完全役か判定
            if (quest.Establish.OnlyPerfectRole && dangos.Count != currentMaxDango)
            {
                quest.ResetContinueCount();
                return false;
            }
            //指定色があるか判定
            if (!HasReadColor(quest, dangos.Distinct()))
            {
                quest.ResetContinueCount();
                return false;
            }

            //作った回数をカウントして…
            quest.AddMadeCount();
            //さらに連続した回数をカウントして…
            quest.AddContinueCount();

            //指定回数作ったか判定
            if (!quest.IsAchievedMadeCount()) return false;
            if (!quest.IsAchievedContinueCount()) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedSr(QuestCreateRole quest, Role<int> role)
        {
            //不正なアクセスであれば弾く
            if (quest.CRType != QuestCreateRole.CreateRoleType.SpecifyTheRole) return false;

            //食べた団子がクエスト内容と一致しているか判定
            //役が一致しているか判定
            if (quest.SpecifyRole.RoleName != role.GetRolename()) return false;

            //作った回数をカウントして…
            quest.AddMadeCount();

            //指定回数作ったか判定
            if (!quest.IsAchievedMadeCount()) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedIc(QuestCreateRole quest, List<DangoColor> colors)
        {
            //不正なアクセスであれば弾く
            if (quest.CRType != QuestCreateRole.CreateRoleType.IncludeColor) return false;

            //色の数を判定
            if (colors.Distinct().Count() != quest.IncludeColors.ColorCount) return false;

            //作った回数をカウントして…
            quest.AddMadeCount();

            //指定回数作ったか判定
            if (!quest.IsAchievedMadeCount()) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedSm(QuestCreateRole quest, Role<int> role)
        {
            //不正なアクセスであれば弾く
            if (quest.CRType != QuestCreateRole.CreateRoleType.CreateSameRole) return false;

            if (!quest.SameRole.IsEqualRole(role))
            {
                quest.ResetContinueCount();

                return false;
            }

            quest.AddContinueCount();

            //指定回数作ったか判定
            if (!quest.IsAchievedContinueCount()) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);

            return true;
        }
        #endregion

        #region PlayAction
        public bool CheckQuestPlayActionSucceed(QuestManager questManager, QuestPlayAction.PlayerAction action)
        {
            //このフレームに別のクエストがクリアされていたら弾く
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestPlayAction questPla)
                {
                    if (CheckQuestSucceed(questPla, action)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestPlayAction quest, QuestPlayAction.PlayerAction action)
        {
            //判定したいアクションが異なったら弾く
            if (quest.Action != action) return false;

            quest.AddMadeCount();

            //指定回数作ったか判定
            if (!quest.IsAchievedMadeCount()) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region Destination
        public bool CheckQuestDestinationSucceed(FloorManager.Floor floor, bool inFloor)
        {
            //このフレームに別のクエストがクリアされていたら弾く
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (_manager.GetQuest(i) is QuestDestination questDest)
                {
                    if (CheckQuestSucceed(questDest, floor, inFloor)) return true;
                }
            }

            return false;
        }

        public bool CheckQuestDestinationSucceed()
        {
            //このフレームに別のクエストがクリアされていたら弾く
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (_manager.GetQuest(i) is QuestDestination questDest)
                {
                    if (CheckQuestSucceed(questDest)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestDestination quest, FloorManager.Floor floor, bool inFloor)
        {
            //はじめに現在いるFloorを登録する
            if (inFloor) quest.SetFloor(floor);

            //目的地でなければ弾く
            if (!quest.Floors.Contains(floor)) return false;

            //部屋の出入りを記録
            quest.SetIsInFloor(inFloor);

            //目的地につくだけでいいのか、ついて食べないといけないのか判定
            if (quest.SucceedOnEat) return false;

            if (!quest.IsInFloor) return false;

            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceed(QuestDestination quest)
        {
            //目的地でなければ弾く
            if (!quest.Floors.Contains(quest.CurrentFloor)) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        private async void QuestSucceed(QuestData quest)
        {
            SoundManager.Instance.PlaySE(SoundSource.SE12_QUEST_SUCCEED);

            List<QuestData> nextQuest = new();
            for (int i = 0; i < quest.NextQuestId.Count; i++)
            {
                nextQuest.Add(Stage001Data.Instance.QuestData[quest.NextQuestId[i]]);
            }

            _manager.ChangeQuest(nextQuest);
            _manager.Player.GrowStab(quest.EnableDangoCountUp);
            _manager.Player.AddSatiety(quest.RewardTime);
            _manager.CreateExpansionUIObj();

            //このフレームで他の判定は行わないようにする処理
            isSucceedThisFrame = true;
            SetBoolAfterOneFrame(false).Forget();

            if (quest.IsKeyQuest)
            {
                //TODO:S7に遷移
            }

            Logger.Log(quest.QuestName + " クエストクリア！");

            //クエストを達成したときの演出
            _playerUIManager.EventText.TextData.SetText("団道達成");
            _playerUIManager.EventText.TextData.SetFontSize(210f);
            
            await _playerUIManager.EventText.TextData.Fadeout(0.5f, 2f);

            _playerUIManager.EventText.TextData.SetFontSize(_playerUIManager.defaultEventTextFontSize);
        }
    }
}