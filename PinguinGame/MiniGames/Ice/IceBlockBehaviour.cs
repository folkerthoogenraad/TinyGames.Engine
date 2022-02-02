using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Util;
using Microsoft.Xna.Framework;

namespace PinguinGame.MiniGames.Ice
{
    public abstract class IceBlockBehaviour
    {
        public abstract string Name { get; }

        public void Update(IEnumerable<IceBlock> blocks, float delta)
        {
            UpdateRelevant(blocks.Where(x => x.Behaviour == Name), delta);
        }

        public abstract void UpdateRelevant(IEnumerable<IceBlock> blocks, float delta);
    }

    public class RandomSinkIceBlockBehaviour : IceBlockBehaviour
    {
        public override string Name => "RandomSink";

        public float SinkTimer = 0;

        public override void UpdateRelevant(IEnumerable<IceBlock> blocks, float delta)
        {
            SinkTimer -= delta;

            if (SinkTimer < 0)
            {
                SinkTimer += 1 + (float)(new Random().NextDouble()) * 3;

                var avail = blocks.Where(x => x.State is IceBlockIdleState);

                if (avail.Count() > 0)
                {
                    var block = avail.Random();
                    block.State = new IceBlockSinkingState(block);
                }
            }
        }
    }

    public class TimedSinkIceBlockBehaviour : IceBlockBehaviour
    {
        public override string Name => "TimedSink";

        public float Timer = 0;

        public override void UpdateRelevant(IEnumerable<IceBlock> blocks, float delta)
        {
            Timer += delta;

            foreach (var block in blocks)
            {
                float localTime = (Timer + block.TimerOffset) % block.TimerCycleDuration;

                if (localTime > block.TimerTrigger && block.IsIdle)
                {
                    block.State = new IceBlockSinkingState(block);
                }
                if (localTime < block.TimerTrigger && block.IsSunken)
                {
                    block.State = new IceBlockRaisingState(block);
                }
            }
        }
    }

    public class TimedDriftIceBlockBehaviour : IceBlockBehaviour
    {
        public override string Name => "TimedDrift";

        public float Timer = 0;

        private float DriftDurationSpeed = 1;

        public override void UpdateRelevant(IEnumerable<IceBlock> blocks, float delta)
        {
            Timer += delta;

            foreach (var block in blocks)
            {
                float localTime = (Timer + block.TimerOffset) % block.TimerCycleDuration;

                Vector2 targetPosition = Vector2.Zero;

                if (localTime > block.TimerTrigger)
                {
                    targetPosition = block.DriftDirection;
                }
                if (localTime < block.TimerTrigger)
                {
                    targetPosition = Vector2.Zero;
                }

                var oldPos = block.Position;
                block.Position = Vector2.Lerp(block.Position, targetPosition, DriftDurationSpeed * delta);
                block.Velocity = (block.Position - oldPos) / delta;
            }
        }
    }
}
