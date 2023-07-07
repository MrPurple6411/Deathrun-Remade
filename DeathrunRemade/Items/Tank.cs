using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using HootLib;
using static CraftData;

namespace DeathrunRemade.Items
{
    /// <summary>
    /// An overall class for representing all special oxygen tanks.
    /// </summary>
    internal class Tank : DeathrunPrefabBase
    {
        public enum Variant
        {
            ChemosynthesisTank,
            PhotosynthesisTank,
            PhotosynthesisTankSmall,
        }

        public Variant TankVariant;
        
        public Tank(Variant variant)
        {
            TankVariant = variant;
            
            _prefabInfo = Hootils.CreatePrefabInfo(
                ItemInfo.GetIdForItem(variant.ToString()),
                GetDisplayName(variant),
                GetDescription(variant),
                GetSprite(variant)
            );

            _prefab = new CustomPrefab(_prefabInfo);
            _prefab.SetRecipe(GetRecipe(variant))
                .WithFabricatorType(CraftTree.Type.Workbench)
                .WithStepsToFabricatorTab(ItemInfo.GetTankCraftTabId());
            _prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
            _prefab.SetEquipment(EquipmentType.Tank);
            _prefabInfo.WithSizeInInventory(new Vector2int(2, 3));
            // The small tank is unlocked earlier and easier to acquire than the two bigger ones.
            TechType unlock = variant == Variant.PhotosynthesisTankSmall ? TechType.Rebreather : TechType.PlasteelTank;
            _prefab.SetUnlock(unlock);

            TechType cloneType = variant == Variant.PhotosynthesisTankSmall ? TechType.Tank : TechType.PlasteelTank;
            var template = new CloneTemplate(_prefabInfo, cloneType);
            _prefab.SetGameObject(template);
            _prefab.Register();
        }
        
        /// <summary>
        /// Gets the display name for the type of tank.
        /// </summary>
        private string GetDisplayName(Variant variant)
        {
            return variant switch
            {
                Variant.ChemosynthesisTank => "Chemosynthesis Tank",
                Variant.PhotosynthesisTank => "Photosynthesis Tank",
                Variant.PhotosynthesisTankSmall => "Small Photosynthesis Tank",
                _ => null
            };
        }

        /// <summary>
        /// Gets the description for the type of tank.
        /// </summary>
        private string GetDescription(Variant variant)
        {
            return variant switch
            {
                Variant.ChemosynthesisTank => "A lightweight O2 tank that houses microorganisms that produce oxygen under high temperatures.",
                Variant.PhotosynthesisTank => "A lightweight air tank housing microorganisms which produce oxygen when exposed to sunlight..",
                Variant.PhotosynthesisTankSmall => "An air tank housing microorganisms which produce oxygen when exposed to sunlight.",
                _ => null
            };
        }
        
        /// <summary>
        /// Gets the recipe for the tank.
        /// </summary>
        private RecipeData GetRecipe(Variant variant)
        {
            RecipeData recipe = variant switch
            {
                Variant.ChemosynthesisTank => new RecipeData(
                    new Ingredient(TechType.PlasteelTank, 1),
                    new Ingredient(ItemInfo.GetTechTypeForItem(nameof(MobDrop.Variant.ThermophileSample)), 4),
                    new Ingredient(TechType.Kyanite, 1)),
                Variant.PhotosynthesisTank => new RecipeData(
                    new Ingredient(TechType.PlasteelTank, 1),
                    new Ingredient(TechType.PurpleBrainCoralPiece, 2),
                    new Ingredient(TechType.EnameledGlass, 1)),
                Variant.PhotosynthesisTankSmall => new RecipeData(
                    new Ingredient(TechType.Tank, 1),
                    new Ingredient(TechType.PurpleBrainCoralPiece, 1),
                    new Ingredient(TechType.Glass, 1)),
                _ => null
            };
            return recipe;
        }

        /// <summary>
        /// Gets the right sprite for the tank.
        /// </summary>
        private Atlas.Sprite GetSprite(Variant variant)
        {
            string filePath = variant switch
            {
                Variant.ChemosynthesisTank => "chemosynthesistank.png",
                Variant.PhotosynthesisTank => "photosynthesistank.png",
                Variant.PhotosynthesisTankSmall => "photosynthesissmalltank.png",
                _ => null
            };
            return Hootils.LoadSprite(filePath, true);
        }
    }
}