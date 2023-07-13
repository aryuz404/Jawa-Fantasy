using UnityEngine;

namespace NotSlot.HandPainted2D.Jungle
{
  [RequireComponent(typeof(ParticleSystem))]
  public sealed class FogFX : MonoBehaviour
  {
    #region Inspector

    [SerializeField]
    [HideInInspector]
    private Material effectMaterial = default;

    [SerializeField]
    private Gradient colors = default;

    [SerializeField]
    [Range(0.5f, 10)]
    private float size = 3;

    [SerializeField]
    [Range(0.05f, 5)]
    private float speed = 0.1f;

    [SerializeField]
    [Range(1, 30)]
    private float lifetime = 20;

    [SerializeField]
    [Range(0.5f, 20)]
    private float reteOverTime = 1;

    [SerializeField]
    [Range(0, 30)]
    private float width = 20;

    [SerializeField]
    [Range(0, 20)]
    private float depth = 4;

    #endregion


    #region Fields

    private ParticleSystem _particles;

    #endregion


    #region MonoBehaviour

    private void OnValidate ()
    {
      if ( _particles == null )
      {
        _particles = GetComponent<ParticleSystem>();
        GetComponent<ParticleSystemRenderer>().sharedMaterial = effectMaterial;
      }

      ParticleSystem.ShapeModule shape = _particles.shape;
      shape.scale = new Vector3(width, depth, 0);

      ParticleSystem.MainModule main = _particles.main;
      main.startSize3D = true;
      main.startSizeX = size * 2;
      main.startSizeY = size;
      main.startSizeZ = 1;
      main.startLifetime = lifetime;
      main.startColor = colors;

      ParticleSystem.VelocityOverLifetimeModule velocity =
        _particles.velocityOverLifetime;
      velocity.x = new ParticleSystem.MinMaxCurve(-speed / 2, speed / 2);
      velocity.y = new ParticleSystem.MinMaxCurve(speed, speed);
      velocity.z = new ParticleSystem.MinMaxCurve(0, 0);

      ParticleSystem.EmissionModule emission = _particles.emission;
      emission.rateOverTime = new ParticleSystem.MinMaxCurve(reteOverTime);
    }

    private void Reset ()
    {
      OnValidate();
    }

    #endregion
  }
}