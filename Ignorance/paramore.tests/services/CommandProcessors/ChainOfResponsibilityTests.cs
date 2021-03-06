﻿using System.Linq;
using Machine.Specifications;
using Paramore.Infrastructure.Domain;
using Paramore.Infrastructure.Raven;
using Paramore.Services.CommandHandlers;
using Paramore.Services.CommandProcessors;
using Paramore.Tests.services.CommandProcessors.TestDoubles;
using TinyIoC;

namespace Paramore.Tests.services.CommandProcessors
{
    [Subject(typeof(ChainofResponsibilityBuilder<>))]
    public class When_Finding_A_Handler_For_A_Command
    {
        private static ChainofResponsibilityBuilder<MyCommand> Chain_Builder;
        private static IHandleRequests<MyCommand> Chain_Of_Responsibility;

        Establish context = () =>
        {
            var container = new TinyIoCContainer();
            container.Register<IHandleRequests<MyCommand>, MyCommandHandler>().AsMultiInstance();            

            Chain_Builder = new ChainofResponsibilityBuilder<MyCommand>(container); 
        };

        Because of = () => Chain_Of_Responsibility = Chain_Builder.Build().First();

        It should_return_the_my_command_handler_as_the_implicit_handler = () => Chain_Of_Responsibility.ShouldBeOfType(typeof(MyCommandHandler));
        It should_be_the_only_element_in_the_chain = () => GetChain().ToString().ShouldEqual("MyCommandHandler|");

        private static ChainPathExplorer GetChain()
        {
            var chainpathExplorer = new ChainPathExplorer();
            Chain_Of_Responsibility.DescribePath(chainpathExplorer);
            return chainpathExplorer;
        }
    }

    [Subject(typeof(ChainofResponsibilityBuilder<>))]
    public class When_Finding_A_Hander_That_Has_Dependencies
    {
        private static ChainofResponsibilityBuilder<MyCommand> Chain_Builder;
        private static IHandleRequests<MyCommand> Chain_Of_Responsibility;

        Establish context = () =>
        {
            var container = new TinyIoCContainer();

            container.Register<IUnitOfWork, MyUnitOfWork>().AsMultiInstance();
            container.Register<IRepository<MyEntity, MyEntityDTO>, Repository<MyEntity, MyEntityDTO>>().AsMultiInstance();
            container.Register<IHandleRequests<MyCommand>, MyDependentCommandHandler>().AsMultiInstance();

            Chain_Builder = new ChainofResponsibilityBuilder<MyCommand>(container);
        };

        Because of = () => Chain_Of_Responsibility = Chain_Builder.Build().First();

        It should_return_the_command_handler_as_the_implicit_handler = () => Chain_Of_Responsibility.ShouldBeOfType(typeof(MyDependentCommandHandler));
        It should_be_the_only_element_in_the_chain = () => GetChain().ToString().ShouldEqual("MyDependentCommandHandler|");

        private static ChainPathExplorer GetChain()
        {
            var chainpathExplorer = new ChainPathExplorer();
            Chain_Of_Responsibility.DescribePath(chainpathExplorer);
            return chainpathExplorer;
        }      
    }

    [Subject(typeof(ChainofResponsibilityBuilder<>))]
    public class When_A_Handler_Is_Part_of_A_Chain_of_Repsonsibility
    {
        private static ChainofResponsibilityBuilder<MyCommand> chainBuilder;
        private static IHandleRequests<MyCommand> chainOfResponsibility;

        Establish context = () =>
        {
            var container = new TinyIoCContainer();
            container.Register<IHandleRequests<MyCommand>, MyImplicitHandler>().AsMultiInstance();

            chainBuilder = new ChainofResponsibilityBuilder<MyCommand>(container);
        };

        Because of = () => chainOfResponsibility = chainBuilder.Build().First();

        It should_include_my_command_handler_filter_in_the_chain = () => GetChain().ToString().Contains("MyImplicitHandler").ShouldBeTrue();
        It should_include_my_logging_handler_in_the_chain = () => GetChain().ToString().Contains("MyLoggingHander").ShouldBeTrue();

        private static ChainPathExplorer GetChain()
        {
            var chainpathExplorer = new ChainPathExplorer();
            chainOfResponsibility.DescribePath(chainpathExplorer);
            return chainpathExplorer;
        }
    }

    [Subject(typeof(ChainofResponsibilityBuilder<>))]
    public class When_Building_A_Chain_of_Repsonsibility_Preserve_The_Order
    {
        private static ChainofResponsibilityBuilder<MyCommand> chainBuilder;
        private static IHandleRequests<MyCommand> chainOfResponsibility;

        Establish context = () =>
        {
            var container = new TinyIoCContainer();
            container.Register<IHandleRequests<MyCommand>, MyDoubleDecoratedHandler>().AsMultiInstance();
            chainBuilder = new ChainofResponsibilityBuilder<MyCommand>(container);
        };

        Because of = () => chainOfResponsibility = chainBuilder.Build().First();

        It should_add_handlers_in_the_correct_sequence_into_the_chain = () => GetChain().ToString().ShouldEqual("MyLoggingHander`1|MyValidationHandler`1|MyDoubleDecoratedHandler|");

        private static ChainPathExplorer GetChain()
        {
            var chainpathExplorer = new ChainPathExplorer();
            chainOfResponsibility.DescribePath(chainpathExplorer);
            return chainpathExplorer;
        }
    }

    [Subject(typeof(ChainofResponsibilityBuilder<>))]
    public class When_Building_A_Chain_of_Repsonsibility_Allow_Pre_And_Post_Tasks
    {
        private static ChainofResponsibilityBuilder<MyCommand> chainBuilder;
        private static IHandleRequests<MyCommand> chainOfResponsibility;

        Establish context = () =>
        {
            var container = new TinyIoCContainer();
            container.Register<IHandleRequests<MyCommand>, MyPreAndPostDecoratedHandler>().AsMultiInstance();
            chainBuilder = new ChainofResponsibilityBuilder<MyCommand>(container);
        };

        Because of = () => chainOfResponsibility = chainBuilder.Build().First();

        It should_add_handlers_in_the_correct_sequence_into_the_chain = () => GetChain().ToString().ShouldEqual("MyValidationHandler`1|MyPreAndPostDecoratedHandler|MyLoggingHander`1|");

        private static ChainPathExplorer GetChain()
        {
            var chainpathExplorer = new ChainPathExplorer();
            chainOfResponsibility.DescribePath(chainpathExplorer);
            return chainpathExplorer;
        }
    }
    
}
