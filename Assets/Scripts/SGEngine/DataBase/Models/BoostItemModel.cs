using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;

public class BoostPlayerItemModel : IItem 
{
    public int Id { get; set; }
    public string InternalName { get; set; }
    public int BasePrice { get; set; }
    public float MultiplyStepPriceByCount { get; set; }
    public int BaseEffectCount { get; set; }
    public int MaxBuyCount { get; set; }
    public string IconPath { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int UserCount { get; set; }

    public int FinalPrice
    {
        get
        {
            if(UserCount == 0)
            {
                return BasePrice;
            } 
            else
            {
                float finalPrice = BasePrice;
                for(int i = 0; i < UserCount; i++)
                {
                    finalPrice *= MultiplyStepPriceByCount;
                }
                return (int)(finalPrice);
            }
        }
    }

    public int FinalEffectNow
    {
        get
        {
            return BaseEffectCount * UserCount;
        }
    }

    public static implicit operator SaveBoostItemModel(BoostPlayerItemModel Item) 
    {
        return new SaveBoostItemModel { Id = Item.Id, UserCount = Item.UserCount };
    }

    public int GetId()
    {
        return Id;
    }


}
