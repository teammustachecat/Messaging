using System;
using NUnit.Framework;
using MustacheCat.Core.Messaging;

namespace MessagingTest
{
	[TestFixture()]
	public class MessageBusTest
	{
		struct MyStringMessage {
			public string name;
			public MyStringMessage(string name) {
				this.name = name;
			}
		}

		struct MyIntMessage {
			public int num;
			public MyIntMessage(int num) {
				this.num = num;
			}
		}

		[Test()]
		public void TestMessaging() {
			var bus = new MessageBus();
			string writeName = "";
			int writeNum = 0;

			var unsubscribe = bus.Subscribe<MyStringMessage>((evt) => writeName = evt.name);
			bus.Trigger(new MyStringMessage("Such Name"));
			Assert.AreEqual("Such Name", writeName);

			Action<MyIntMessage> updateNum = (evt) => writeNum = evt.num;
			bus.Subscribe(updateNum);
			bus.Trigger(new MyIntMessage(42));
			Assert.AreEqual(42, writeNum);

			bus.Trigger(new MyStringMessage("Such New Name"));
			Assert.AreEqual("Such New Name", writeName);

			unsubscribe();
			bus.Trigger(new MyStringMessage("The Latest Name?"));
			Assert.AreEqual("Such New Name", writeName);

			bus.Unsubscribe(updateNum);
			bus.Trigger(new MyIntMessage(7));
			Assert.AreEqual(42, writeNum);
		}
	}
}
