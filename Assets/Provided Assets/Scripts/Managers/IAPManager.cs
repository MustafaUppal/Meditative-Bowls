using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace MeditativeBowls
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        public static IAPManager instance;

        private static IStoreController m_StoreController;
        private static IExtensionProvider m_StoreExtensionProvider;

        [System.Serializable]
        public class CustomProduct
        {
            public string id = "com.HimalayanBowls.SingingBowls.Set2.Carpet1";
            public UnityEngine.Purchasing.ProductType type = UnityEngine.Purchasing.ProductType.NonConsumable;
        }

        [Header("Purchase Failure Reasons")]
        public string[] purchaseFailureReasons = new string[8]
        {
            "Purchase Unavailable",
            "Existing Purchase Pending",
            "Product Unavailable",
            "Signature Invalid",
            "User Cancelled",
            "Payment Declined",
            "Duplicate Transaction",
            "Something went wrong"
        };

        //Step 1 create your products
        // public CustomProduct[] carpets;
        public CustomProduct[] bowls;

        // public CustomProduct[] slideShows;

        InventoryManager Inventory => InventoryManager.Instance;


        //************************** Adjust these methods **************************************
        public void InitializePurchasing()
        {
            if (IsInitialized()) { return; }
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());



            //Step 2 choose if your product is a consumable or non consumable
            // for (int i = 0; i < carpets.Length; i++)
            // {
            //     carpets[i].id = Inventory.GetItemProductId(0, i);
            //     builder.AddProduct(carpets[i].id, carpets[i].type);
            // }

            for (int i = 0; i < bowls.Length; i++)
            {
                // bowls[i].id = Inventory.GetItemProductId(1, i);
                builder.AddProduct(bowls[i].id, bowls[i].type);
            }

            // for (int i = 0; i < slideShows.Length; i++)
            // {
            //     slideShows[i].id = Inventory.GetItemProductId(2, i);
            //     builder.AddProduct(slideShows[i].id, slideShows[i].type);
            // }

            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }


        //Step 3 Create methods
        public void PurchaseItem(int index, int type)
        {
            PopupManager.Instance.spinnerLoading.Show("Preparing Purchase");
            string productId = "";

            switch (type)
            {
                case 0: // carpet
                    // productId = carpets[index].id;
                    break;
                case 1: // bowl
                    productId = bowls[index].id;
                    break;
                case 2: // slideshow
                    // productId = slideShows[index].id;
                    break;
            }

            // Debug.Log("Purchasing: " + productId);
            BuyProductID(productId);
        }



        //Step 4 modify purchasing
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            bool productFound = false;

            Debug.Log("ProcessPurchase");
            // check slideshow
            // for (int i = 0; i < slideShows.Length && !productFound; i++)
            // {
            //     if (String.Equals(args.purchasedProduct.definition.id, slideShows[i].id, StringComparison.Ordinal))
            //     {
            //         productFound = true;

            //         AllRefs.I.shopMenu.OnItemPurchased(2, i);
            //         break;
            //     }
            // }

            // check carpets
            // for (int i = 0; i < carpets.Length && !productFound; i++)
            // {
            //     if (String.Equals(args.purchasedProduct.definition.id, carpets[i].id, StringComparison.Ordinal))
            //     {
            //         // Debug.Log("Purchased: " + carpets[i].id);
            //         productFound = true;

            //         AllRefs.I.shopMenu.OnItemPurchased(0, i);
            //         break;
            //     }
            // }

            // check bowls
            for (int i = 0; i < bowls.Length && !productFound; i++)
            {
                Debug.Log("Finding Product: " + bowls[i].id);

                if (String.Equals(args.purchasedProduct.definition.id, bowls[i].id, StringComparison.Ordinal))
                {
                    // Debug.Log("Purchased: " + bowls[i].id);
                    productFound = true;
                    AllRefs.I.shopMenu.OnItemPurchased(1, i);
                    break;
                }
            }

            if (!productFound)
            {
                Debug.Log("Purchase Failed");
            }

            PopupManager.Instance.spinnerLoading.Hide();

            return PurchaseProcessingResult.Complete;
        }

        //**************************** Dont worry about these methods ***********************************
        private void Awake()
        {
            TestSingleton();
        }

        void Start()
        {
            Debug.Log("Object Created");
            if (m_StoreController == null) { InitializePurchasing(); }
        }

        static int count;
        private void TestSingleton()
        {
            // Debug.Log("IAP: " + count++);
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Debug.Log("IAP1: " + count++);
        }

        void BuyProductID(string productId)
        {
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(productId);
                if (product != null && product.availableToPurchase)
                {
                    // Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    m_StoreController.InitiatePurchase(product);
                    // result is shwon in ProcessPurchase or OnPurchaseFailed functions
                }
                else
                {
                    PopupManager.Instance.spinnerLoading.Hide();
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                PopupManager.Instance.spinnerLoading.Hide();
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        public void RestorePurchases()
        {
            if (!IsInitialized())
            {
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                Debug.Log("RestorePurchases started ...");

                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                apple.RestoreTransactions((result) =>
                {
                    AllRefs.I.shopMenu.restoreButton.SetActive(false);
                    PlayerPreferencesManager.SetItemsRestored(true);
                    PopupManager.Instance.messagePopup.Show("Purchases Restored!", "All your you previous purchases has been restored.");
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            else
            {
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized: PASS");
            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // if duplication purchase
            if ((int)failureReason == 6)
            {
                bool productFound = false;

                // check slideshow
                // for (int i = 0; i < slideShows.Length && !productFound; i++)
                // {
                //     if (String.Equals(product.definition.storeSpecificId, slideShows[i].id, StringComparison.Ordinal))
                //     {
                //         Debug.Log("Purchased: " + slideShows[i].id);
                //         productFound = true;

                //         AllRefs.I.shopMenu.OnItemPurchased(2, i);
                //         break;
                //     }
                // }

                // check carpets
                // for (int i = 0; i < carpets.Length && !productFound; i++)
                // {
                //     if (String.Equals(product.definition.storeSpecificId, carpets[i].id, StringComparison.Ordinal))
                //     {
                //         Debug.Log("Purchased: " + carpets[i].id);
                //         productFound = true;

                //         AllRefs.I.shopMenu.OnItemPurchased(0, i);
                //         break;
                //     }
                // }

                // check bowls
                for (int i = 0; i < bowls.Length && !productFound; i++)
                {
                    if (String.Equals(product.definition.storeSpecificId, bowls[i].id, StringComparison.Ordinal))
                    {
                        Debug.Log("Purchased: " + bowls[i].id);
                        productFound = true;

                        AllRefs.I.shopMenu.OnItemPurchased(1, i);
                        break;
                    }
                }
            }

            PopupManager.Instance.messagePopup.Show("Purchase Failed!", purchaseFailureReasons[(int)failureReason]);
            PopupManager.Instance.spinnerLoading.Hide();

            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
    }

}