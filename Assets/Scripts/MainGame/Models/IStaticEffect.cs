public interface IStaticEffect
{
    delegate void RemoveEffect();
    event RemoveEffect isEffectNeedRomove;

    void SetEffectController(EffectObjectController effectObjectController);
}
