namespace FakeItEasy.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using FakeItEasy.Configuration;
    using FakeItEasy.Core;
    using FakeItEasy.Tests.TestHelpers;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ReturnValueConfigurationExtensionsTests
    {
        public interface IInterface
        {
            int RequestOfOne(int number);

            string RequestOfOne(string text);

            [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Required for testing.")]
            string RequestOfOneWithOutput(out string text);

            [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Required for testing.")]
            string RequestOfOneWithReference(ref string text);

            int RequestOfTwo(int number1, int number2);

            string RequestOfTwo(string text1, string text2);

            [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Required for testing.")]
            [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Required for testing.")]
            string RequestOfTwoWithOutputAndReference(out string text1, ref string text2);

            int RequestOfThree(int number1, int number2, int number3);

            string RequestOfThree(string text1, string text2, string text3);

            [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Required for testing.")]
            [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Required for testing.")]
            string RequestOfThreeWithOutputAndReference(out string text1, ref string text2, string text3);

            int RequestOfFour(int number1, int number2, int number3, int number4);

            string RequestOfFour(string text1, string text2, string text3, string text4);

            [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Required for testing.")]
            [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Required for testing.")]
            string RequestOfFourWithOutputAndReference(string text1, string text2, ref string text3, out string text4);

            int RequestOfFive(int number1, int number2, int number3, int number4, int number5);

            string RequestOfFive(string text1, string text2, string text3, string text4, string text5);

            [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Required for testing.")]
            [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Required for testing.")]
            string RequestOfFiveWithOutputAndReference(string text1, string text2, ref string text3, ref string text4, out string text5);

            int RequestOfSix(int number1, int number2, int number3, int number4, int number5, int number6);

            string RequestOfSix(string text1, string text2, string text3, string text4, string text5, string text6);

            [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Required for testing.")]
            [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Required for testing.")]
            string RequestOfSixWithOutputAndReference(string text1, string text2, ref string text3, ref string text4, ref string text5, out string text6);
        }

        [Test]
        public void Returns_should_return_configuration_returned_from_passed_in_configuration()
        {
            // Arrange
            var expectedConfig = A.Fake<IAfterCallSpecifiedWithOutAndRefParametersConfiguration>();
            var config = A.Fake<IReturnValueConfiguration<int>>();
            A.CallTo(() => config.ReturnsLazily(A<Func<IFakeObjectCall, int>>.That.Matches(x => x.Invoke(null) == 10))).Returns(expectedConfig);

            // Act
            var returned = config.Returns(10);

            // Assert
            returned.Should().BeSameAs(expectedConfig);
        }

        [Test]
        public void Returns_should_return_configuration_returned_from_passed_in_configuration_task()
        {
            // Arrange
            var expectedConfig = A.Fake<IAfterCallSpecifiedWithOutAndRefParametersConfiguration>();
            var config = A.Fake<IReturnValueConfiguration<Task<int>>>();
            A.CallTo(() => config.ReturnsLazily(A<Func<IFakeObjectCall, Task<int>>>.That.Matches(x => x.Invoke(null).Result == 10))).Returns(expectedConfig);

            // Act
            var returned = config.Returns(10);

            // Assert
            returned.Should().BeSameAs(expectedConfig);
        }

        [Test]
        public void Returns_should_be_null_guarded()
        {
            // Arrange

            // Act

            // Assert
            NullGuardedConstraint.Assert(() =>
                A.Fake<IReturnValueConfiguration<string>>().Returns(null));
        }

        [Test]
        public void ReturnsLazily_with_1_argument_should_use_returns_lazily_ReturnsLazily_with_action_having_1_argument()
        {
            // Arrange
            const int Argument = 2;
            const int ReturnValue = 5;
            int? collectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfOne(Argument)).ReturnsLazily((int i) =>
                {
                    collectedArgument = i;
                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfOne(Argument);

            // Assert
            result.Should().Be(ReturnValue);
            collectedArgument.Should().Be(Argument);
        }

        [Test]
        public void ReturnsLazily_with_1_argument_should_support_overloads()
        {
            // Arrange
            const string Argument = "argument";
            const string ReturnValue = "Result";
            string collectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfOne(Argument)).ReturnsLazily((string s) =>
                {
                    collectedArgument = s;
                    return ReturnValue;
                });
            
            // Act
            var result = fake.RequestOfOne(Argument);

            result.Should().Be(ReturnValue);
            collectedArgument.Should().Be(Argument);
        }

        [Test]
        public void ReturnsLazily_with_1_argument_should_support_out_parameter()
        {
            // Arrange
            const string ReturnValue = "Result";
            string argument = "argument";
            string collectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfOneWithOutput(out argument)).ReturnsLazily((string s) =>
            {
                collectedArgument = s;
                return ReturnValue;
            });
            
            // Act
            var result = fake.RequestOfOneWithOutput(out argument);

            // Assert
            result.Should().Be(ReturnValue);
            collectedArgument.Should().Be(argument);
        }

        [Test]
        public void ReturnsLazily_with_1_argument_should_support_ref_parameter()
        {
            // Arrange
            const string ReturnValue = "Result";
            string argument = "argument";
            string collectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfOneWithReference(ref argument)).ReturnsLazily((string s) =>
            {
                collectedArgument = s;
                return ReturnValue;
            });

            // Act
            var result = fake.RequestOfOneWithReference(ref argument);

            // Assert
            result.Should().Be(ReturnValue);
            collectedArgument.Should().Be(argument);
        }

        [Test]
        public void ReturnsLazily_with_1_argument_should_throw_exception_when_argument_count_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfTwo(A<int>._, A<int>._))
                .ReturnsLazily((int i) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfTwo(5, 8);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32)", "(System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_1_argument_should_throw_exception_when_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfOne(A<int>._))
                .ReturnsLazily((string s) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfOne(5);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32)", "(System.String)");
        }

        [Test]
        public void ReturnsLazily_with_2_arguments_should_use_returns_lazily_ReturnsLazily_with_action_having_2_arguments()
        {
            // Arrange
            const int FirstArgument = 5;
            const int SecondArgument = 8;
            const int ReturnValue = 0;

            int? firstCollectedArgument = null;
            int? secondCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfTwo(A<int>._, A<int>._))
                .ReturnsLazily((int i, int j) =>
                    {
                        firstCollectedArgument = i;
                        secondCollectedArgument = j;

                        return ReturnValue;
                    });

            // Act
            var result = fake.RequestOfTwo(FirstArgument, SecondArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().HaveValue().And.Be(FirstArgument);
            secondCollectedArgument.Should().HaveValue().And.Be(SecondArgument);
        }

        [Test]
        public void ReturnsLazily_with_2_arguments_should_support_overloads()
        {
            // Arrange
            const string FirstArgument = "first argument";
            const string SecondArgument = "second argument";
            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfTwo(A<string>._, A<string>._))
                .ReturnsLazily((string s, string t) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfTwo(FirstArgument, SecondArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
        }

        [Test]
        public void ReturnsLazily_with_2_arguments_should_support_out_and_ref()
        {
            // Arrange
            const string ReturnValue = "Result";
            string firstArgument = "first argument";
            string secondArgument = "second argument";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfTwoWithOutputAndReference(out firstArgument, ref secondArgument))
                .ReturnsLazily((string s, string t) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfTwoWithOutputAndReference(out firstArgument, ref secondArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(firstArgument);
            secondCollectedArgument.Should().Be(secondArgument);
        }

        [Test]
        public void ReturnsLazily_with_2_arguments_should_throw_exception_when_argument_count_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfOne(A<int>._))
                .ReturnsLazily((int i, int j) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfOne(5);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32)", "(System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_2_arguments_should_throw_exception_when_first_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfTwo(A<int>._, A<int>._))
                .ReturnsLazily((string s, int i) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfTwo(5, 8);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32)", "(System.String, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_2_arguments_should_throw_exception_when_second_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfTwo(A<int>._, A<int>._))
                .ReturnsLazily((int i, string s) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfTwo(5, 8);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32)", "(System.Int32, System.String)");
        }

        [Test]
        public void ReturnsLazily_with_3_arguments_should_use_returns_lazily_ReturnsLazily_with_action_having_3_arguments()
        {
            // Arrange
            const int FirstArgument = 5;
            const int SecondArgument = 8;
            const int ThirdArgument = 13;
            const int ReturnValue = 0;

            int? firstCollectedArgument = null;
            int? secondCollectedArgument = null;
            int? thirdCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfThree(A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k) =>
                {
                    firstCollectedArgument = i;
                    secondCollectedArgument = j;
                    thirdCollectedArgument = k;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfThree(FirstArgument, SecondArgument, ThirdArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
            thirdCollectedArgument.Should().Be(ThirdArgument);
        }

        [Test]
        public void ReturnsLazily_with_3_arguments_should_support_overloads()
        {
            // Arrange
            const string FirstArgument = "first argument";
            const string SecondArgument = "second argument";
            const string ThirdArgument = "third argument";
            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;
            string thirdCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfThree(A<string>._, A<string>._, A<string>._))
                .ReturnsLazily((string s, string t, string u) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;
                    thirdCollectedArgument = u;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfThree(FirstArgument, SecondArgument, ThirdArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
            thirdCollectedArgument.Should().Be(ThirdArgument);
        }

        [Test]
        public void ReturnsLazily_with_3_arguments_should_support_out_and_ref()
        {
            // Arrange
            string firstArgument = "first argument";
            string secondArgument = "second argument";
            const string ThirdArgument = "third argument";
            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;
            string thirdCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfThreeWithOutputAndReference(out firstArgument, ref secondArgument, A<string>._))
                .ReturnsLazily((string s, string t, string u) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;
                    thirdCollectedArgument = u;

                    return ReturnValue;
                });
            
            // Act
            var result = fake.RequestOfThreeWithOutputAndReference(out firstArgument, ref secondArgument, ThirdArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(firstArgument);
            secondCollectedArgument.Should().Be(secondArgument);
            thirdCollectedArgument.Should().Be(ThirdArgument);
        }

        [Test]
        public void ReturnsLazily_with_3_arguments_should_throw_exception_when_argument_count_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfTwo(A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfTwo(5, 8);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_3_arguments_should_throw_exception_when_first_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfThree(A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((string s, int i, int j) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfThree(5, 8, 13);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32)", "(System.String, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_3_arguments_should_throw_exception_when_second_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfThree(A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, string s, int j) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfThree(5, 8, 13);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32)", "(System.Int32, System.String, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_3_arguments_should_throw_exception_when_third_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfThree(A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, string s, int j) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfThree(5, 8, 13);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32)", "(System.Int32, System.String, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_4_arguments_should_use_returns_lazily_ReturnsLazily_with_action_having_4_arguments()
        {
            // Arrange
            const int FirstArgument = 5;
            const int SecondArgument = 8;
            const int ThirdArgument = 13;
            const int FourthArgument = 21;
            const int ReturnValue = 0;

            int? firstCollectedArgument = null;
            int? secondCollectedArgument = null;
            int? thirdCollectedArgument = null;
            int? fourthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFour(A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l) =>
                {
                    firstCollectedArgument = i;
                    secondCollectedArgument = j;
                    thirdCollectedArgument = k;
                    fourthCollectedArgument = l;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfFour(FirstArgument, SecondArgument, ThirdArgument, FourthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().HaveValue().And.Be(FirstArgument);
            secondCollectedArgument.Should().HaveValue().And.Be(SecondArgument);
            thirdCollectedArgument.Should().HaveValue().And.Be(ThirdArgument);
            fourthCollectedArgument.Should().HaveValue().And.Be(FourthArgument);
        }

        [Test]
        public void ReturnsLazily_with_4_arguments_should_support_overloads()
        {
            // Arrange
            const string FirstArgument = "first argument";
            const string SecondArgument = "second argument";
            const string ThirdArgument = "third argument";
            const string FourthArgument = "fourth argument";
            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;
            string thirdCollectedArgument = null;
            string fourthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFour(A<string>._, A<string>._, A<string>._, A<string>._))
                .ReturnsLazily((string s, string t, string u, string v) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;
                    thirdCollectedArgument = u;
                    fourthCollectedArgument = v;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfFour(FirstArgument, SecondArgument, ThirdArgument, FourthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
            thirdCollectedArgument.Should().Be(ThirdArgument);
            fourthCollectedArgument.Should().Be(FourthArgument);
        }

        [Test]
        public void ReturnsLazily_with_4_arguments_should_support_out_and_ref()
        {
            // Arrange
            const string FirstArgument = "first argument";
            const string SecondArgument = "second argument";
            string thirdArgument = "third argument";
            string fourthArgument = "fourth argument";
            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;
            string thirdCollectedArgument = null;
            string fourthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFourWithOutputAndReference(A<string>._, A<string>._, ref thirdArgument, out fourthArgument))
                .ReturnsLazily((string s, string t, string u, string v) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;
                    thirdCollectedArgument = u;
                    fourthCollectedArgument = v;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfFourWithOutputAndReference(FirstArgument, SecondArgument, ref thirdArgument, out fourthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
            thirdCollectedArgument.Should().Be(thirdArgument);
            fourthCollectedArgument.Should().Be(fourthArgument);
        }

        [Test]
        public void ReturnsLazily_with_4_arguments_should_throw_exception_when_argument_count_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfThree(A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfThree(5, 8, 13);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_4_arguments_should_throw_exception_when_first_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFour(A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((string s, int i, int j, int k) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFour(5, 8, 13, 21);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32)", "(System.String, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_4_arguments_should_throw_exception_when_second_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFour(A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, string s, int j, int k) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFour(5, 8, 13, 21);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.String, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_4_arguments_should_throw_exception_when_third_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFour(A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, string s, int k) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFour(5, 8, 13, 21);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.String, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_4_arguments_should_throw_exception_when_fourth_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFour(A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, string s) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFour(5, 8, 13, 21);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.String)");
        }

        [Test]
        public void ReturnsLazily_with_5_arguments_should_use_returns_lazily_ReturnsLazily_with_action_having_5_arguments()
        {
            // Arrange
            const int FirstArgument = 5;
            const int SecondArgument = 8;
            const int ThirdArgument = 13;
            const int FourthArgument = 21;
            const int FifthArgument = 34;
            const int ReturnValue = 0;

            int? firstCollectedArgument = null;
            int? secondCollectedArgument = null;
            int? thirdCollectedArgument = null;
            int? fourthCollectedArgument = null;
            int? fifthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFive(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l, int m) =>
                {
                    firstCollectedArgument = i;
                    secondCollectedArgument = j;
                    thirdCollectedArgument = k;
                    fourthCollectedArgument = l;
                    fifthCollectedArgument = m;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfFive(FirstArgument, SecondArgument, ThirdArgument, FourthArgument, FifthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().HaveValue().And.Be(FirstArgument);
            secondCollectedArgument.Should().HaveValue().And.Be(SecondArgument);
            thirdCollectedArgument.Should().HaveValue().And.Be(ThirdArgument);
            fourthCollectedArgument.Should().HaveValue().And.Be(FourthArgument);
            fifthCollectedArgument.Should().HaveValue().And.Be(FifthArgument);
        }

        [Test]
        public void ReturnsLazily_with_5_arguments_should_support_overloads()
        {
            // Arrange
            const string FirstArgument = "first argument";
            const string SecondArgument = "second argument";
            const string ThirdArgument = "third argument";
            const string FourthArgument = "fourth argument";
            const string FifthArgument = "fifth argument";

            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;
            string thirdCollectedArgument = null;
            string fourthCollectedArgument = null;
            string fifthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFive(A<string>._, A<string>._, A<string>._, A<string>._, A<string>._))
                .ReturnsLazily((string s, string t, string u, string v, string w) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;
                    thirdCollectedArgument = u;
                    fourthCollectedArgument = v;
                    fifthCollectedArgument = w;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfFive(FirstArgument, SecondArgument, ThirdArgument, FourthArgument, FifthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
            thirdCollectedArgument.Should().Be(ThirdArgument);
            fourthCollectedArgument.Should().Be(FourthArgument);
            fifthCollectedArgument.Should().Be(FifthArgument);
        }

        [Test]
        public void ReturnsLazily_with_5_arguments_should_support_out_and_ref()
        {
            // Arrange
            const string FirstArgument = "first argument";
            const string SecondArgument = "second argument";
            string thirdArgument = "third argument";
            string fourthArgument = "fourth argument";
            string fifthArgument = "fifth argument";
            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;
            string thirdCollectedArgument = null;
            string fourthCollectedArgument = null;
            string fifthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFiveWithOutputAndReference(A<string>._, A<string>._, ref thirdArgument, ref fourthArgument, out fifthArgument))
                .ReturnsLazily((string s, string t, string u, string v, string w) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;
                    thirdCollectedArgument = u;
                    fourthCollectedArgument = v;
                    fifthCollectedArgument = w;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfFiveWithOutputAndReference(FirstArgument, SecondArgument, ref thirdArgument, ref fourthArgument, out fifthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
            thirdCollectedArgument.Should().Be(thirdArgument);
            fourthCollectedArgument.Should().Be(fourthArgument);
            fifthCollectedArgument.Should().Be(fifthArgument);
        }
        
        [Test]
        public void ReturnsLazily_with_5_arguments_should_throw_exception_when_argument_count_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFour(A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l, int m) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFour(5, 8, 13, 21);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_5_arguments_should_throw_exception_when_first_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFive(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((string s, int i, int j, int k, int l) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFive(5, 8, 13, 21, 34);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.String, System.Int32, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_5_arguments_should_throw_exception_when_second_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFive(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, string s, int j, int k, int l) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFive(5, 8, 13, 21, 34);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.String, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_5_arguments_should_throw_exception_when_third_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFive(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, string s, int k, int l) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFive(5, 8, 13, 21, 34);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.String, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_5_arguments_should_throw_exception_when_forth_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFive(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, string s, int l) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFive(5, 8, 13, 21, 34);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.String, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_5_arguments_should_throw_exception_when_fifth_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFive(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l, string s) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFive(5, 8, 13, 21, 34);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.Int32, System.String)");
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_use_returns_lazily_ReturnsLazily_with_action_having_6_arguments()
        {
            // Arrange
            const int FirstArgument = 5;
            const int SecondArgument = 8;
            const int ThirdArgument = 13;
            const int FourthArgument = 21;
            const int FifthArgument = 34;
            const int SixthArgument = 55;
            const int ReturnValue = 0;

            int? firstCollectedArgument = null;
            int? secondCollectedArgument = null;
            int? thirdCollectedArgument = null;
            int? fourthCollectedArgument = null;
            int? fifthCollectedArgument = null;
            int? sixthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSix(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l, int m, int n) =>
                {
                    firstCollectedArgument = i;
                    secondCollectedArgument = j;
                    thirdCollectedArgument = k;
                    fourthCollectedArgument = l;
                    fifthCollectedArgument = m;
                    sixthCollectedArgument = n;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfSix(FirstArgument, SecondArgument, ThirdArgument, FourthArgument, FifthArgument, SixthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().HaveValue().And.Be(FirstArgument);
            secondCollectedArgument.Should().HaveValue().And.Be(SecondArgument);
            thirdCollectedArgument.Should().HaveValue().And.Be(ThirdArgument);
            fourthCollectedArgument.Should().HaveValue().And.Be(FourthArgument);
            fifthCollectedArgument.Should().HaveValue().And.Be(FifthArgument);
            sixthCollectedArgument.Should().HaveValue().And.Be(SixthArgument);
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_support_overloads()
        {
            // Arrange
            const string FirstArgument = "first argument";
            const string SecondArgument = "second argument";
            const string ThirdArgument = "third argument";
            const string FourthArgument = "fourth argument";
            const string FifthArgument = "fifth argument";
            const string SixthArgument = "sixth argument";

            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;
            string thirdCollectedArgument = null;
            string fourthCollectedArgument = null;
            string fifthCollectedArgument = null;
            string sixthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSix(A<string>._, A<string>._, A<string>._, A<string>._, A<string>._, A<string>._))
                .ReturnsLazily((string s, string t, string u, string v, string w, string x) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;
                    thirdCollectedArgument = u;
                    fourthCollectedArgument = v;
                    fifthCollectedArgument = w;
                    sixthCollectedArgument = x;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfSix(FirstArgument, SecondArgument, ThirdArgument, FourthArgument, FifthArgument, SixthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
            thirdCollectedArgument.Should().Be(ThirdArgument);
            fourthCollectedArgument.Should().Be(FourthArgument);
            fifthCollectedArgument.Should().Be(FifthArgument);
            sixthCollectedArgument.Should().Be(SixthArgument);
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_support_out_and_ref()
        {
            // Arrange
            const string FirstArgument = "first argument";
            const string SecondArgument = "second argument";
            string thirdArgument = "third argument";
            string fourthArgument = "fourth argument";
            string fifthArgument = "fifth argument";
            string sixthArgument = "sixth argument";
            const string ReturnValue = "Result";

            string firstCollectedArgument = null;
            string secondCollectedArgument = null;
            string thirdCollectedArgument = null;
            string fourthCollectedArgument = null;
            string fifthCollectedArgument = null;
            string sixthCollectedArgument = null;

            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSixWithOutputAndReference(A<string>._, A<string>._, ref thirdArgument, ref fourthArgument, ref fifthArgument, out sixthArgument))
                .ReturnsLazily((string s, string t, string u, string v, string w, string x) =>
                {
                    firstCollectedArgument = s;
                    secondCollectedArgument = t;
                    thirdCollectedArgument = u;
                    fourthCollectedArgument = v;
                    fifthCollectedArgument = w;
                    sixthCollectedArgument = x;

                    return ReturnValue;
                });

            // Act
            var result = fake.RequestOfSixWithOutputAndReference(FirstArgument, SecondArgument, ref thirdArgument, ref fourthArgument, ref fifthArgument, out sixthArgument);

            // Assert
            result.Should().Be(ReturnValue);
            firstCollectedArgument.Should().Be(FirstArgument);
            secondCollectedArgument.Should().Be(SecondArgument);
            thirdCollectedArgument.Should().Be(thirdArgument);
            fourthCollectedArgument.Should().Be(fourthArgument);
            fifthCollectedArgument.Should().Be(fifthArgument);
            sixthCollectedArgument.Should().Be(sixthArgument);
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_throw_exception_when_argument_count_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfFive(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l, int m, int n) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfFive(5, 8, 13, 21, 34);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_throw_exception_when_first_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSix(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((string s, int i, int j, int k, int l, int m) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfSix(5, 8, 13, 21, 34, 55);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.String, System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_throw_exception_when_second_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSix(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, string s, int j, int k, int l, int m) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfSix(5, 8, 13, 21, 34, 55);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.String, System.Int32, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_throw_exception_when_third_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSix(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, string s, int k, int l, int m) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfSix(5, 8, 13, 21, 34, 55);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.String, System.Int32, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_throw_exception_when_fourth_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSix(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, string s, int l, int m) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfSix(5, 8, 13, 21, 34, 55);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.String, System.Int32, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_throw_exception_when_fifth_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSix(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l, string s, int m) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfSix(5, 8, 13, 21, 34, 55);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.Int32, System.String, System.Int32)");
        }

        [Test]
        public void ReturnsLazily_with_6_arguments_should_throw_exception_when_sixth_argument_type_does_not_match()
        {
            // Arrange
            var fake = A.Fake<IInterface>();
            A.CallTo(() => fake.RequestOfSix(A<int>._, A<int>._, A<int>._, A<int>._, A<int>._, A<int>._))
                .ReturnsLazily((int i, int j, int k, int l, int m, string s) => { throw new InvalidOperationException("returns lazily action should not be executed"); });
            Action act = () => fake.RequestOfSix(5, 8, 13, 21, 34, 55);

            // Act, Assert
            AssertThatSignatureMismatchExceptionIsThrown(act, "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)", "(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32, System.String)");
        }

        [Test]
        public void Curried_ReturnsLazily_returns_value_from_curried_function()
        {
            // Arrange
            var config = A.Fake<IReturnValueConfiguration<int>>();
            int currentValue = 10;

            // Act
            config.ReturnsLazily(() => currentValue);

            // Assert
            var curriedFunction = Fake.GetCalls(config).Single().Arguments.Get<Func<IFakeObjectCall, int>>(0);

            curriedFunction.Invoke(A.Dummy<IFakeObjectCall>()).Should().Be(currentValue);
            currentValue = 20;
            curriedFunction.Invoke(A.Dummy<IFakeObjectCall>()).Should().Be(currentValue);
        }

        [Test]
        public void Curried_ReturnsLazily_should_be_null_guarded()
        {
            // Arrange

            // Act

            // Assert
            NullGuardedConstraint.Assert(() =>
                A.Fake<IReturnValueConfiguration<int>>().ReturnsLazily(() => 10));
        }

        [Test]
        public void ReturnsNextFromSequence_should_call_returns_with_factory_that_returns_next_from_sequence_for_each_call()
        {
            // Arrange
            var sequence = new[] { 1, 2, 3 };
            var config = A.Fake<IReturnValueConfiguration<int>>();
            var call = A.Fake<IFakeObjectCall>();

            // Act
            config.ReturnsNextFromSequence(sequence);

            // Assert
            Func<Func<IFakeObjectCall, int>> factoryValidator = () => A<Func<IFakeObjectCall, int>>.That.Matches(
                x =>
                {
                    var producedSequence = new[] { x.Invoke(call), x.Invoke(call), x.Invoke(call) };
                    return producedSequence.SequenceEqual(sequence);
                },
                "Predicate");

            A.CallTo(() => config.ReturnsLazily(factoryValidator.Invoke())).MustHaveHappened();
        }

        [Test]
        public void ReturnsNextFromSequence_should_set_repeat_to_the_number_of_values_in_sequence()
        {
            // Arrange
            var config = A.Fake<IReturnValueConfiguration<int>>();
            var returnedConfig = A.Fake<IAfterCallSpecifiedWithOutAndRefParametersConfiguration>();

            A.CallTo(() => config.ReturnsLazily(A<Func<IFakeObjectCall, int>>._)).Returns(returnedConfig);

            // Act
            config.ReturnsNextFromSequence(1, 2, 3);

            // Assert
            A.CallTo(() => returnedConfig.NumberOfTimes(3)).MustHaveHappened();
        }

        private static void AssertThatSignatureMismatchExceptionIsThrown(Action act, string fakeSignature, string returnsLazilySignature)
        {
            // Arrange
            var expectedMessage = "The faked method has the signature " + fakeSignature + ", but returns lazily was used with " + returnsLazilySignature + ".";
            
            var exception = Record.Exception(act);

            exception.Should().BeOfType<FakeConfigurationException>();
            exception.Message.Should().Be(expectedMessage);
        }
    }
}