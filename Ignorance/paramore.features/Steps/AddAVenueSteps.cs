﻿using System;
using Paramore.Infrastructure.Raven;
using Paramore.Services.CommandHandlers;
using Paramore.Services.CommandHandlers.Venues;
using Paramore.Services.CommandProcessors;
using Paramore.Services.Commands.Venue;
using TechTalk.SpecFlow;
using TinyIoC;

namespace Paramore.Features.Steps
{
    [Binding]
    public class AddAVenue
    {
        private static TinyIoCContainer container;
        private static CommandProcessor commandProcessor;
        private static AddVenueCommand command;

        [BeforeFeature]
        public static void SetUp()
        {
            command = new AddVenueCommand();

            container = new TinyIoCContainer();

            commandProcessor = new CommandProcessor(container);
            container.Register<IAmAUnitOfWorkFactory, UnitOfWorkFactory>().AsSingleton();
            container.Register<IHandleRequests<AddVenueCommand>, AddVenueCommandHandler>("ScheduleMeetingCommandHandler");
        }

        [Given(@"That I have a venue name of (.*)")]
        public void GivenThatIHaveAVenueNameOf(string venueName)
        {
            command.VenueName = venueName;
        }

        [When(@"I create a new venue")]
        public void WhenICreateANewVenue()
        {
            command.VenueId = Guid.NewGuid();
            commandProcessor.Send(command);
        }

        [Then(@"whe I list venues (.*) should be included")]
        public void ThenWheIListVenuesShouldBeIncluded(string venueName)
        {
            ScenarioContext.Current.Pending();
        }
    }

    [Binding]
    public class AddAnAddressToAVenue
    {
        [Given(@"I have a street address of (.*)")]
        public void GivenIHaveAStreetAddressOf116_120GoswellRoad()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"a city of (.*)")]
        public void GivenACityOf(string city)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"a post code of (.*)")]
        public void GivenAPostCodeOf(string postCode)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I add an address to a venue and ask for directions")]
        public void WhenIAddAnAddressToAVenueAndAskForDirections()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I should get a street address of (.*)")]
        public void ThenIShouldGetAStreetAddressOf()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"a city of (.*)")]
        public void ThenACityOf(string city)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"a post code of (.*)")]
        public void ThenAPostCodeOf(string postCode)
        {
            ScenarioContext.Current.Pending();
        }
    }

    [Binding]
    public class AddAMapToAVenue
    {
        [Given(@"I have a map uri of (.*)")]
        public void GivenIHaveAMapUriOfHttpSkillsmatter_ComGoFind_Us(string mapUri)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I add a map to a location and ask for directions")]
        public void WhenIAddAMapToALocationAndAskForDirections()
        {
            ScenarioContext.Current.Pending();
        }


        [Then(@"I should get a map uri of (.*)")]
        public void ThenIShouldGetAMapUriOfHttpSkillsmatter_ComGoFind_Us(string mapUri)
        {
            ScenarioContext.Current.Pending();
        }
    }

}
