﻿using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.Cluster.SplitBrainResolver
{
    public class SplitBrainResolverDowningProvider : StrategizedDowningProvider
    {
        public SplitBrainResolverDowningProvider(ActorSystem system)
            : base(system, "split-brain-resolver")
        {

        }

        protected override IDowningStrategy GetDowningStrategy()
        {
            var config = System.Settings.Config;

            var requestedStrategy = 
                config.GetString("akka.cluster.split-brain-resolver.active-strategy");

            IDowningStrategy strategy = null;

            switch(requestedStrategy)
            {
                case "static-quorum":
                    strategy = new StaticQuorumDowningStrategy(config);
                    break;
                case "keep-referee":
                    strategy = new KeepRefereeDowningStrategy(config);
                    break;
                case "keep-majority":
                    strategy = new KeepMajorityDowningStrategy(config);
                    break;
                case "keep-oldest":
                    strategy = new KeepOldestDowningStrategy(config);
                    break;
                case "off":
                    strategy = new NoopDowningStrategy();
                    break;
                default:
                    throw new NotSupportedException($"Unknown downing strategy requested");
            }

            return strategy;
        }
    }
}
