using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FeatureInspection;

namespace Feature_Inspection.UnitTests
{
	[TestFixture]
	class FinalInspectionPresenterTests
	{
		private Mock<IFinalInspection> viewMock;
		private Mock<IFinalInspectionModel> modelMock;

		private FinalInspectionPresenter sut;

		[SetUp]
		public void SetUp()
		{

			viewMock = new Mock<IFinalInspection>(MockBehavior.Strict);
			modelMock = new Mock<IFinalInspectionModel>(MockBehavior.Strict);

			sut = new FinalInspectionPresenter(viewMock.Object, modelMock.Object);
		}


	}
}
