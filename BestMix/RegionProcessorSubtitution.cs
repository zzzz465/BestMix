using System;
using System.Collections.Generic;
using Harmony;

using Verse;
using RimWorld;

using BestMix.Patches;

namespace BestMix
{
    public abstract class RegionProcessorSubtitution
    {
        #region static methods and fields
        public static RegionProcessorSubtitution singleton { get; private set; }

        public static void Initialize(RegionProcessorSubtitution instance)
        {
            if(singleton != null)
                throw new Exception("RegionProcessorSubtitution should be initialized once! you're calling initializer more than once.");

            singleton = instance;
        }

        #endregion

        protected List<ThingCount> chosenIngThings { get; set; } // 바뀜
        protected IntRange ReCheckFailedBillTicksRange { get; } // 안바뀜
		protected string MissingMaterialsTranslated { get; } // 안바뀜
		protected List<Thing> relevantThings { get; set; } // 바뀔수도
		protected HashSet<Thing> processedThings { get; set; } // 바뀔수도
		protected List<Thing> newRelevantThings { get; set; } // 바뀔수도
		protected List<IngredientCount> ingredientsOrdered { get; set; } // 바뀜
		protected List<Thing> tmpMedicine { get; set; } // 필요없음
        protected int lf_adjacentRegionsAvailable { get; set; }
        protected int lf_regionsProcessed { get; set; }
        protected IntVec3 lf_rootCell { get; set; }
        protected Bill p_bill { get; set; }
        protected Pawn p_pawn { get; set; }
        protected Thing p_billGiver { get; set; }
        protected List<ThingCount> p_chosen { get; set; }
        protected bool lf_foundAll { get; set; }

        //class option
        protected virtual bool ShouldOverrideValuesToOriginalClass { get; } = false;
        protected virtual bool CopyOnOverride { get; } = false;
		//protected WorkGiver_DoBill.DefCountList availableCounts { get; set; }


        protected RegionProcessorSubtitution()
        {

        }

        // called by reflection, connected by Patch_WorkGiver_DoBill
        private void FetchData(List<ThingCount> _chosenIngThings,
                               List<Thing> _relevantThings,
                               HashSet<Thing> _processedThings,
                               List<Thing> _newReleventThings,
                               List<IngredientCount> _ingredientsOrdered,
                               int _adjacentRegionsAvailable,
                               int _regionsProcessed,
                               IntVec3 _rootCell,
                               Bill _bill,
                               Pawn _pawn,
                               Thing _billGiver,
                               List<ThingCount> _chosen,
                               bool _foundAll)
        {
            this.chosenIngThings = _chosenIngThings;
            this.relevantThings = _relevantThings;
            this.processedThings = _processedThings;
            this.newRelevantThings = _newReleventThings;
            this.ingredientsOrdered = _ingredientsOrdered;
            this.lf_adjacentRegionsAvailable = _adjacentRegionsAvailable;
            this.lf_regionsProcessed = _regionsProcessed;
            this.lf_rootCell = _rootCell;
            this.p_bill = _bill;
            this.p_pawn = _pawn;
            this.p_billGiver = _billGiver;
            this.p_chosen = _chosen;
            this.lf_foundAll = _foundAll;
        }

        protected abstract bool RegionProcessor(Region reg);

        // called by reflection, connected by Patch_WorkGiver_DoBill
        private void UpdateData(ref List<ThingCount> _chosenIngThings,
                               ref List<Thing> _relevantThings,
                               ref HashSet<Thing> _processedThings,
                               ref List<Thing> _newReleventThings,
                               ref List<IngredientCount> _ingredientsOrdered)
        {
            if(!ShouldOverrideValuesToOriginalClass)
                return;

            if(CopyOnOverride)
            {
                _chosenIngThings = new List<ThingCount>(this.chosenIngThings);
                _relevantThings = new List<Thing>(this.relevantThings);
                _processedThings = new HashSet<Thing>(this.processedThings);
                _newReleventThings = new List<Thing>(this.newRelevantThings);
                _ingredientsOrdered = new List<IngredientCount>(this.ingredientsOrdered);
            }
            else
            {
                _chosenIngThings = this.chosenIngThings;
                _relevantThings = this.relevantThings;
                _processedThings = this.processedThings;
                _newReleventThings = this.newRelevantThings;
                _ingredientsOrdered = this.ingredientsOrdered;
            }
        }
    }
}