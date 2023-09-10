using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpChargeEffect_ParticleSystem : JumpChargeEffect
{
    [SerializeField]
    private new ParticleSystem particleSystem;

    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emission;

    

    [System.Serializable]
    private class ParticleSettings
    {
        public Gradient colorOverIntensity;
        public float emissionMultiplier;
        public AnimationCurve emissionOverIntensity = AnimationCurve.Linear(0, 0, 1, 1);
    }

    [SerializeField]
    private ParticleSettings chargeSettings;
    [SerializeField]
    private ParticleSettings jumpSettings;
    [SerializeField]
    private ParticleSettings hoverSettings;
    [SerializeField]
    private ParticleSettings stopSettings;

    private ParticleSettings currentSettings;

    [SerializeField]
    private float baseRateOverTimeMultiplier;

    private float intensity;

    protected override void Start()
    {
        main = particleSystem.main;
        emission = particleSystem.emission;

        currentSettings = new ParticleSettings();

        currentSettings.colorOverIntensity = chargeSettings.colorOverIntensity;
        currentSettings.emissionMultiplier = chargeSettings.emissionMultiplier;
    }

    public override void SetIntensity(float intensity)
    {
        this.intensity = intensity;
        UpdateIntensity();
    }

    private void UpdateIntensity()
    {
        emission.rateOverTimeMultiplier = baseRateOverTimeMultiplier * currentSettings.emissionOverIntensity.Evaluate(intensity) * currentSettings.emissionMultiplier;
        main.startColor = currentSettings.colorOverIntensity.Evaluate(intensity);
    }

    public override void StartCharge()
    {
        LerpSettings(chargeSettings, 0.1f);
    }

    public override void StartHover()
    {
        LerpSettings(hoverSettings, 0.25f);
    }

    public override void StartJump()
    {
        LerpSettings(jumpSettings, 0.08f);
    }

    public override void Stop()
    {
        LerpSettings(stopSettings, 0.1f);
    }

    private void LerpSettings(ParticleSettings target, float time)
    {
        currentSettings = target;
        UpdateIntensity();
    }


}

