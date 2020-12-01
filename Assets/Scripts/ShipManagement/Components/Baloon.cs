using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShipManagement.Components
{
    public class Baloon : ShipComponent
    {
        private const float EFFECTIVE_HEIGHT_MIN = 25f;
        private const float EFFECTIVE_HEIGHT_MAX = 45f;

        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite coldSprite;

        private SpriteRenderer renderer;
        
        public override string Name => nameof(Baloon);
        public override string Description => $"Has negative weight {Weight}. Loses its effectiveness at altitude " +
                                              $"{(int)EFFECTIVE_HEIGHT_MIN} or higher. Gives {Toughness} toughness. Cost {Cost}";
        
        public override int Cost => 4;
        public override int Toughness => -4;
        public override int Weight => -3;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            
            StartCoroutine(CheckEffectivenessCoroutine());
        }

        private void Update()
        {
            if (transform.position.y > EFFECTIVE_HEIGHT_MIN)
                renderer.sprite = coldSprite;
            else
                renderer.sprite = normalSprite;
        }

        private IEnumerator CheckEffectivenessCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                if (IsEnabled && transform.position.y > EFFECTIVE_HEIGHT_MIN)
                {
                    if (transform.position.y > EFFECTIVE_HEIGHT_MAX)
                    {
                        Pop();
                        yield break;
                    }

                    float e = (transform.position.y - EFFECTIVE_HEIGHT_MIN) /
                              (EFFECTIVE_HEIGHT_MAX - EFFECTIVE_HEIGHT_MIN);
                    if (Random.Range(0, 1f) < e)
                    {
                        Pop();
                        yield break;
                    }
                }
            }
        }

        private void Pop()
        {
            Ship.ForceRemoveComponent(this);
        }
    }
}