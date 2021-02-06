using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkOperatingConditionsServiceTest
    {
        private readonly Mock<IMarkOperatingConditionsRepo> _repository = new Mock<IMarkOperatingConditionsRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IOperatingAreaRepo> _mockOperatingAreaRepo = new Mock<IOperatingAreaRepo>();
        private readonly Mock<IGasGroupRepo> _mockGasGroupRepo = new Mock<IGasGroupRepo>();
        private readonly Mock<IEnvAggressivenessRepo> _mockEnvAggressivenessRepo = new Mock<IEnvAggressivenessRepo>();
        private readonly Mock<IConstructionMaterialRepo> _mockConstructionMaterialRepo = new Mock<IConstructionMaterialRepo>();
        private readonly Mock<IPaintworkTypeRepo> _mockPaintworkTypeRepo = new Mock<IPaintworkTypeRepo>();
        private readonly Mock<IHighTensileBoltsTypeRepo> _mockHighTensileBoltsTypeRepo = new Mock<IHighTensileBoltsTypeRepo>();
        private readonly IMarkOperatingConditionsService _service;
        private readonly Random _rnd = new Random();
        private readonly List<MarkOperatingConditions> _markOperatingConditions =
            new List<MarkOperatingConditions> { };
        private readonly int _maxMarkId = 2;

        public MarkOperatingConditionsServiceTest()
        {
            // Arrange
            foreach (var moc in TestData.markOperatingConditions)
            {
                _markOperatingConditions.Add(new MarkOperatingConditions
                {
                    Mark = moc.Mark,
                    MarkId = moc.MarkId,
                    SafetyCoeff = moc.SafetyCoeff,
                    EnvAggressiveness = moc.EnvAggressiveness,
                    Temperature = moc.Temperature,
                    OperatingArea = moc.OperatingArea,
                    GasGroup = moc.GasGroup,
                    ConstructionMaterial = moc.ConstructionMaterial,
                    PaintworkType = moc.PaintworkType,
                    HighTensileBoltsType = moc.HighTensileBoltsType,
                });
            }

            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetByMarkId(mark.Id)).Returns(
                        _markOperatingConditions.SingleOrDefault(v => v.Mark.Id == mark.Id));
            }
            foreach (var envAggressiveness in TestData.envAggressiveness)
            {
                _mockEnvAggressivenessRepo.Setup(mock =>
                    mock.GetById(envAggressiveness.Id)).Returns(
                        TestData.envAggressiveness.SingleOrDefault(
                            v => v.Id == envAggressiveness.Id));
            }
            foreach (var operatingArea in TestData.operatingAreas)
            {
                _mockOperatingAreaRepo.Setup(mock =>
                    mock.GetById(operatingArea.Id)).Returns(
                        TestData.operatingAreas.SingleOrDefault(v => v.Id == operatingArea.Id));
            }
            foreach (var gasGroup in TestData.gasGroups)
            {
                _mockGasGroupRepo.Setup(mock =>
                    mock.GetById(gasGroup.Id)).Returns(
                        TestData.gasGroups.SingleOrDefault(v => v.Id == gasGroup.Id));
            }
            foreach (var constructionMaterial in TestData.constructionMaterials)
            {
                _mockConstructionMaterialRepo.Setup(mock =>
                    mock.GetById(constructionMaterial.Id)).Returns(
                        TestData.constructionMaterials.SingleOrDefault(
                            v => v.Id == constructionMaterial.Id));
            }
            foreach (var paintworkType in TestData.paintworkTypes)
            {
                _mockPaintworkTypeRepo.Setup(mock =>
                    mock.GetById(paintworkType.Id)).Returns(
                        TestData.paintworkTypes.SingleOrDefault(v => v.Id == paintworkType.Id));
            }
            foreach (var highTensileBoltsType in TestData.highTensileBoltsTypes)
            {
                _mockHighTensileBoltsTypeRepo.Setup(mock =>
                    mock.GetById(highTensileBoltsType.Id)).Returns(
                        TestData.highTensileBoltsTypes.SingleOrDefault(
                            v => v.Id == highTensileBoltsType.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<MarkOperatingConditions>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<MarkOperatingConditions>())).Verifiable();

            _service = new MarkOperatingConditionsService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockOperatingAreaRepo.Object,
                _mockGasGroupRepo.Object,
                _mockEnvAggressivenessRepo.Object,
                _mockConstructionMaterialRepo.Object,
                _mockPaintworkTypeRepo.Object,
                _mockHighTensileBoltsTypeRepo.Object);
        }

        [Fact]
        public void GetByMarkId_ShouldReturnMarkOperatingConditions()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);

            // Act
            var markOperatingConditions = _service.GetByMarkId(markId);

            // Assert
            Assert.NotNull(markOperatingConditions);
        }

        [Fact]
        public void Create_ShouldCreateMarkOperatingConditions()
        {
            // Arrange
            int markId = 3;
            int envAggressivenessId = _rnd.Next(1, TestData.envAggressiveness.Count());
            int operatingAreaId = _rnd.Next(1, TestData.operatingAreas.Count());
            int gasGroupId = _rnd.Next(1, TestData.gasGroups.Count());
            int constructionMaterialId = _rnd.Next(1, TestData.constructionMaterials.Count());
            int paintworkTypeId = _rnd.Next(1, TestData.paintworkTypes.Count());
            int highTensileBoltsTypeId = _rnd.Next(1, TestData.highTensileBoltsTypes.Count());

            var newMarkOperatingConditions = new MarkOperatingConditions
            {
                SafetyCoeff = 1.0f,
                Temperature = -34,
            };

            // Act
            _service.Create(newMarkOperatingConditions,
                markId,
                envAggressivenessId,
                operatingAreaId,
                gasGroupId,
                constructionMaterialId,
                paintworkTypeId,
                highTensileBoltsTypeId);

            // Assert
            _repository.Verify(mock => mock.Add(
                It.IsAny<MarkOperatingConditions>()), Times.Once);
            Assert.NotNull(newMarkOperatingConditions.Mark);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int markId = 3;
            int envAggressivenessId = _rnd.Next(1, TestData.envAggressiveness.Count());
            int operatingAreaId = _rnd.Next(1, TestData.operatingAreas.Count());
            int gasGroupId = _rnd.Next(1, TestData.gasGroups.Count());
            int constructionMaterialId = _rnd.Next(1, TestData.constructionMaterials.Count());
            int paintworkTypeId = _rnd.Next(1, TestData.paintworkTypes.Count());
            int highTensileBoltsTypeId = _rnd.Next(1, TestData.highTensileBoltsTypes.Count());

            var newMarkOperatingConditions = new MarkOperatingConditions
            {
                SafetyCoeff = 1.0f,
                Temperature = -34,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                null,
                markId,
                envAggressivenessId,
                operatingAreaId,
                gasGroupId,
                constructionMaterialId,
                paintworkTypeId,
                highTensileBoltsTypeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newMarkOperatingConditions,
                999,
                envAggressivenessId,
                operatingAreaId,
                gasGroupId,
                constructionMaterialId,
                paintworkTypeId,
                highTensileBoltsTypeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newMarkOperatingConditions,
                markId,
                999,
                operatingAreaId,
                gasGroupId,
                constructionMaterialId,
                paintworkTypeId,
                highTensileBoltsTypeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newMarkOperatingConditions,
                markId,
                envAggressivenessId,
                999,
                gasGroupId,
                constructionMaterialId,
                paintworkTypeId,
                highTensileBoltsTypeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newMarkOperatingConditions,
                markId,
                envAggressivenessId,
                operatingAreaId,
                999,
                constructionMaterialId,
                paintworkTypeId,
                highTensileBoltsTypeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newMarkOperatingConditions,
                markId,
                envAggressivenessId,
                operatingAreaId,
                gasGroupId,
                999,
                paintworkTypeId,
                highTensileBoltsTypeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newMarkOperatingConditions,
                markId,
                envAggressivenessId,
                operatingAreaId,
                gasGroupId,
                constructionMaterialId,
                999,
                highTensileBoltsTypeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newMarkOperatingConditions,
                markId,
                envAggressivenessId,
                operatingAreaId,
                gasGroupId,
                constructionMaterialId,
                paintworkTypeId,
                999));

            _repository.Verify(mock => mock.Add(
                It.IsAny<MarkOperatingConditions>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValue()
        {
            // Arrange
            int markId = 1;
            int envAggressivenessId = _rnd.Next(1, TestData.envAggressiveness.Count());
            int operatingAreaId = _rnd.Next(1, TestData.operatingAreas.Count());
            int gasGroupId = _rnd.Next(1, TestData.gasGroups.Count());
            int constructionMaterialId = _rnd.Next(1, TestData.constructionMaterials.Count());
            int paintworkTypeId = _rnd.Next(1, TestData.paintworkTypes.Count());
            int highTensileBoltsTypeId = _rnd.Next(1, TestData.highTensileBoltsTypes.Count());

            var newMarkOperatingConditions = new MarkOperatingConditions
            {
                SafetyCoeff = 1.0f,
                Temperature = -34,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(newMarkOperatingConditions,
                markId,
                envAggressivenessId,
                operatingAreaId,
                gasGroupId,
                constructionMaterialId,
                paintworkTypeId,
                highTensileBoltsTypeId));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkOperatingConditions>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateMarkOperatingConditions()
        {
            // Arrange
            int markId = 1;
            int envAggressivenessId = _rnd.Next(1, TestData.envAggressiveness.Count());
            int operatingAreaId = _rnd.Next(1, TestData.operatingAreas.Count());
            int gasGroupId = _rnd.Next(1, TestData.gasGroups.Count());
            int constructionMaterialId = _rnd.Next(1, TestData.constructionMaterials.Count());
            int paintworkTypeId = _rnd.Next(1, TestData.paintworkTypes.Count());
            int highTensileBoltsTypeId = _rnd.Next(1, TestData.highTensileBoltsTypes.Count());

            var markOperatingConditionsRequest = new MarkOperatingConditionsUpdateRequest
            {
                SafetyCoeff = 2.0f,
                Temperature = -40,
                EnvAggressivenessId = envAggressivenessId,
                OperatingAreaId = operatingAreaId,
                GasGroupId = gasGroupId,
                ConstructionMaterialId = constructionMaterialId,
                PaintworkTypeId = paintworkTypeId,
                HighTensileBoltsTypeId = highTensileBoltsTypeId,
            };

            // Act
            _service.Update(markId,
                markOperatingConditionsRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<MarkOperatingConditions>()), Times.Once);
            Assert.Equal(envAggressivenessId, _markOperatingConditions.SingleOrDefault(
                v => v.Mark.Id == markId).EnvAggressiveness.Id);
            Assert.Equal(operatingAreaId, _markOperatingConditions.SingleOrDefault(
                v => v.Mark.Id == markId).OperatingArea.Id);
            Assert.Equal(gasGroupId, _markOperatingConditions.SingleOrDefault(
                v => v.Mark.Id == markId).GasGroup.Id);
            Assert.Equal(constructionMaterialId, _markOperatingConditions.SingleOrDefault(
                v => v.Mark.Id == markId).ConstructionMaterial.Id);
            Assert.Equal(paintworkTypeId, _markOperatingConditions.SingleOrDefault(
                v => v.Mark.Id == markId).PaintworkType.Id);
            Assert.Equal(highTensileBoltsTypeId, _markOperatingConditions.SingleOrDefault(
                v => v.Mark.Id == markId).HighTensileBoltsType.Id);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int markId = 1;

            var markOperatingConditionsRequest = new MarkOperatingConditionsUpdateRequest
            {
                SafetyCoeff = 2.0f,
            };
            var wrongMarkOperatingConditionsRequest1 = new MarkOperatingConditionsUpdateRequest
            {
                EnvAggressivenessId = 999,
            };
            var wrongMarkOperatingConditionsRequest2 = new MarkOperatingConditionsUpdateRequest
            {
                OperatingAreaId = 999,
            };
            var wrongMarkOperatingConditionsRequest3 = new MarkOperatingConditionsUpdateRequest
            {
                GasGroupId = 999,
            };
            var wrongMarkOperatingConditionsRequest4 = new MarkOperatingConditionsUpdateRequest
            {
                ConstructionMaterialId = 999,
            };
            var wrongMarkOperatingConditionsRequest5 = new MarkOperatingConditionsUpdateRequest
            {
                PaintworkTypeId = 999,
            };
            var wrongMarkOperatingConditionsRequest6 = new MarkOperatingConditionsUpdateRequest
            {
                HighTensileBoltsTypeId = 999,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(markId, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999,
                markOperatingConditionsRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                markId,
                wrongMarkOperatingConditionsRequest1));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                markId,
                wrongMarkOperatingConditionsRequest2));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                markId,
                wrongMarkOperatingConditionsRequest3));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                markId,
                wrongMarkOperatingConditionsRequest4));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                markId,
                wrongMarkOperatingConditionsRequest5));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                markId,
                wrongMarkOperatingConditionsRequest6));

            _repository.Verify(mock => mock.Update(It.IsAny<MarkOperatingConditions>()), Times.Never);
        }
    }
}
