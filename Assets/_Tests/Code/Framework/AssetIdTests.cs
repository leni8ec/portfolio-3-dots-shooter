using Game.Framework.Assets;
using NUnit.Framework;

namespace Tests.Framework {
    public class AssetIdTests {
        [TestCase("Item", "Sword", "Item_Sword")]
        [TestCase("A", "B", "A_B")]
        [TestCase("", "", "_")]
        [TestCase("", "Sword", "_Sword")]
        [TestCase("Item", "", "Item_")]
        public void Combine_ReturnsExpectedValue(string left, string right, string expected) {
            var id1 = new AssetId(left);
            var id2 = new AssetId(right);

            var result = id1.Combine(id2);

            Assert.AreEqual(expected, result.ToString());
            Assert.LessOrEqual(result.Value.Length, 29);
        }

        [TestCase("VeryLongAssetName12345", "Short", "VeryLongAssetName12345_Short")]
        [TestCase("Short", "VeryLongAssetName12345", "Short_VeryLongAssetName12345")]
        [TestCase("VeryLongAssetName12345", "VeryLongAssetName12345", "VeryLongAssetN_VeryLongAssetN")]
        public void Combine_WhenInputsAreTooLong_TruncatesOnlyWhenCombinedValueDoesNotFit(
            string left,
            string right,
            string expected
        ) {
            var id1 = new AssetId(left);
            var id2 = new AssetId(right);

            var result = id1.Combine(id2);

            Assert.AreEqual(expected, result.ToString());
            Assert.LessOrEqual(result.Value.Length, 29);
        }

        [Test]
        public void StaticCombine_ReturnsSameResultAsInstanceCombine() {
            var left = new AssetId("Item");
            var right = new AssetId("Sword");

            Assert.AreEqual(
                left.Combine(right),
                AssetId.Combine(left, right)
            );
        }

        [Test]
        public void ToString_ReturnsUnderlyingValue() {
            var id = new AssetId("Item");

            Assert.AreEqual("Item", id.ToString());
        }

        [Test]
        public void Equality_WhenValuesAreSame_ReturnsTrue() {
            var id1 = new AssetId("Item");
            var id2 = new AssetId("Item");

            Assert.IsTrue(id1.Equals(id2));
            Assert.IsTrue(id1 == id2);
            Assert.IsFalse(id1 != id2);
        }

        [Test]
        public void Equality_WhenValuesAreDifferent_ReturnsFalse() {
            var id1 = new AssetId("Item");
            var id2 = new AssetId("Sword");

            Assert.IsFalse(id1.Equals(id2));
            Assert.IsFalse(id1 == id2);
            Assert.IsTrue(id1 != id2);
        }

        [Test]
        public void GetHashCode_WhenValuesAreSame_ReturnsSameHash() {
            var id1 = new AssetId("Item");
            var id2 = new AssetId("Item");

            Assert.AreEqual(id1.GetHashCode(), id2.GetHashCode());
        }
    }
}