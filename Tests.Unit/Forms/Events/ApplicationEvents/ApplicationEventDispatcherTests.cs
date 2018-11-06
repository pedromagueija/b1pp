// <copyright filename="ApplicationEventDispatcherTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Forms.Events.ApplicationEvents
{
    using System;

    using B1PP.Forms.Events.ApplicationEvents;

    using NSubstitute;

    using NUnit.Framework;

    using SAPbouiCOM;

    internal class ApplicationEventDispatcherTests
    {
        private Application app;
        private AppEventHandlerTester handler;
        private ApplicationEventDispatcher dispatcher;

        [SetUp]
        public void Setup()
        {
            app = Substitute.For<Application>();
            handler = new AppEventHandlerTester();
            dispatcher = new ApplicationEventDispatcher(handler);
            dispatcher.Subscribe(app);
        }

        [Test]
        public void Subscribe_Links_To_CompanyChanged_Event()
        {
            dispatcher.Subscribe(app);

            app.AppEvent += Raise.Event<_IApplicationEvents_AppEventEventHandler>(BoAppEventTypes.aet_CompanyChanged);

            Assert.IsTrue(handler.CompanyChangedReceived);
            Assert.IsFalse(handler.FontChangedReceived);
            Assert.IsFalse(handler.ShutdownReceived);
            Assert.IsFalse(handler.AddonStoppedReceived);
            Assert.IsFalse(handler.LanguageChangedReceived);
        }

        [Test]
        public void Subscribe_Links_To_FontChanged_Event()
        {
            dispatcher.Subscribe(app);

            app.AppEvent += Raise.Event<_IApplicationEvents_AppEventEventHandler>(BoAppEventTypes.aet_FontChanged);

            Assert.IsTrue(handler.FontChangedReceived);
            Assert.IsFalse(handler.CompanyChangedReceived);
            Assert.IsFalse(handler.ShutdownReceived);
            Assert.IsFalse(handler.AddonStoppedReceived);
            Assert.IsFalse(handler.LanguageChangedReceived);
        }

        [Test]
        public void Subscribe_Links_To_Shutdown_Event()
        {
            dispatcher.Subscribe(app);

            app.AppEvent += Raise.Event<_IApplicationEvents_AppEventEventHandler>(BoAppEventTypes.aet_ShutDown);

            Assert.IsTrue(handler.ShutdownReceived);
            Assert.IsFalse(handler.FontChangedReceived);
            Assert.IsFalse(handler.CompanyChangedReceived);
            Assert.IsFalse(handler.AddonStoppedReceived);
            Assert.IsFalse(handler.LanguageChangedReceived);
        }

        [Test]
        public void Subscribe_Links_To_AddonStopped_Event()
        {
            dispatcher.Subscribe(app);

            app.AppEvent += Raise.Event<_IApplicationEvents_AppEventEventHandler>(BoAppEventTypes.aet_ServerTerminition);

            Assert.IsTrue(handler.AddonStoppedReceived);
            Assert.IsFalse(handler.ShutdownReceived);
            Assert.IsFalse(handler.FontChangedReceived);
            Assert.IsFalse(handler.CompanyChangedReceived);
            Assert.IsFalse(handler.LanguageChangedReceived);
        }

        [Test]
        public void Subscribe_Links_To_LanguageChanged_Event()
        {
            dispatcher.Subscribe(app);

            app.AppEvent += Raise.Event<_IApplicationEvents_AppEventEventHandler>(BoAppEventTypes.aet_LanguageChanged);

            Assert.IsTrue(handler.LanguageChangedReceived);
            Assert.IsFalse(handler.AddonStoppedReceived);
            Assert.IsFalse(handler.ShutdownReceived);
            Assert.IsFalse(handler.FontChangedReceived);
            Assert.IsFalse(handler.CompanyChangedReceived);
        }

        [Test]
        public void Subscribe_After_Unsubscribe_Does_Not_Throw()
        {
            dispatcher.Subscribe(app);

            dispatcher.Unsubscribe();

            Assert.DoesNotThrow(() => dispatcher.Subscribe(app));
        }

        [Test]
        public void Unsubscribe_Unlinks_From_Events()
        {
            dispatcher.Subscribe(app);

            foreach (var evtType in Enum.GetValues(typeof(BoAppEventTypes)))
            {
                app.AppEvent += Raise.Event<_IApplicationEvents_AppEventEventHandler>(evtType);    
            }
            
            Assert.IsTrue(handler.LanguageChangedReceived);
            Assert.IsTrue(handler.AddonStoppedReceived);
            Assert.IsTrue(handler.ShutdownReceived);
            Assert.IsTrue(handler.FontChangedReceived);
            Assert.IsTrue(handler.CompanyChangedReceived);

            handler.ShutdownReceived = false;
            handler.AddonStoppedReceived = false;
            handler.CompanyChangedReceived = false;
            handler.FontChangedReceived = false;
            handler.LanguageChangedReceived = false;

            dispatcher.Unsubscribe();

            foreach (var evtType in Enum.GetValues(typeof(BoAppEventTypes)))
            {
                app.AppEvent += Raise.Event<_IApplicationEvents_AppEventEventHandler>(evtType);    
            }
            
            Assert.IsFalse(handler.LanguageChangedReceived);
            Assert.IsFalse(handler.AddonStoppedReceived);
            Assert.IsFalse(handler.ShutdownReceived);
            Assert.IsFalse(handler.FontChangedReceived);
            Assert.IsFalse(handler.CompanyChangedReceived);
        }

        [Test]
        public void Subscribe_Throws_On_Unknown_Event()
        {
            dispatcher.Subscribe(app);

            Assert.Throws<ArgumentOutOfRangeException>(() => app.AppEvent += Raise.Event<_IApplicationEvents_AppEventEventHandler>((BoAppEventTypes)999));
        }
    }

    internal class AppEventHandlerTester : IApplicationEventsHandler
    {
        public bool CompanyChangedReceived { get; set; }

        public bool AddonStoppedReceived { get; set; }

        public bool FontChangedReceived { get; set; }

        public bool LanguageChangedReceived { get; set; }
        
        public bool ShutdownReceived { get; set; }

        public void OnAddonStopped()
        {
            AddonStoppedReceived = true;
        }

        public void OnCompanyChanged()
        {
            CompanyChangedReceived = true;
        }

        public void OnFontChanged()
        {
            FontChangedReceived = true;
        }

        public void OnLanguageChanged()
        {
            LanguageChangedReceived = true;
        }

        public void OnShutdown()
        {
            ShutdownReceived = true;
        }
    }
}