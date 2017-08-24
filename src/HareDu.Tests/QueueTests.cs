﻿namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_can_create_queue()
        {
            Result result = await Client
                .Factory<Queue>()
                .Create(x =>
                {
                    x.Configure(d =>
                    {
                        d.Name("TestQueue");
                        d.IsDurable();
                        d.WithArguments(arg =>
                        {
                            arg.SetQueueExpiration(1000);
                        });
                    });
                    x.OnNode("MyNode1");
                    x.OnVirtualHost("HareDu");
                });
        }

        [Test]
        public void Verify_can_get_all()
        {
            var result = Client
                .Factory<Queue>()
                .GetAll()
                .Where(x => x.Name == "HareDu");
            
            foreach (var queue in result)
            {
                Console.WriteLine("Name: {0}", queue.Name);
                Console.WriteLine("VirtualHost: {0}", queue.VirtualHost);
                Console.WriteLine("AutoDelete: {0}", queue.AutoDelete);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

        }

        [Test]
        public async Task Verify_can_delete_queue()
        {
            Result result = await Client
                .Factory<Queue>()
                .Delete(x =>
                {
                    x.Queue("");
                    x.OnVirtualHost("HareDu");
                    x.WithConditions(c =>
                    {
                        c.IfUnused();
                        c.IfEmpty();
                    });
                });
        }

        [Test]
        public async Task Verify_can_peek_messages()
        {
            Result result = await Client
                .Factory<Queue>()
                .Peek(x =>
                {
                    x.Queue("");
                    x.OnVirtualHost("HareDu");
                });
        }
    }
}