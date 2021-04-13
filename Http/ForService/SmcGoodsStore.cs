using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http.ForService
{
  
        public class SmcGoodsStore
    {
        string earchValue;
        string createBy;
        string createTime;
        string updateBy;
        string updateTime;
        string remark;
        string paramss;

        string goodsStoreId;
        string goodsId;
        string classifyId;
        string shopId;
        string goodsName;
        string pinyinCode;
        string barcode;
        string showImg;
        string mainImg;
        string imgUrl;
        string costPrice;
        string originalPrice;
        string salePrice;
        string vipPrice;
        string specData;
        string unit;
        string inventoryNow;
        string inventoryMax;
        string inventoryMin;
        string isDiscount;
        string integralGoods;
        string makeDate;
        string shelfLife;
        string status;
        string classifyName;
        string wholesale_price;

        public string EarchValue { get => earchValue; set => earchValue = value; }
        public string CreateBy { get => createBy; set => createBy = value; }
        public string CreateTime { get => createTime; set => createTime = value; }
        public string UpdateBy { get => updateBy; set => updateBy = value; }
        public string UpdateTime { get => updateTime; set => updateTime = value; }
        public string Remark { get => remark; set => remark = value; }
        public string Paramss { get => paramss; set => paramss = value; }
        public string GoodsStoreId { get => goodsStoreId; set => goodsStoreId = value; }
        public string GoodsId { get => goodsId; set => goodsId = value; }
        public string ClassifyId { get => classifyId; set => classifyId = value; }
        public string ShopId { get => shopId; set => shopId = value; }
        public string GoodsName { get => goodsName; set => goodsName = value; }
        public string PinyinCode { get => pinyinCode; set => pinyinCode = value; }
        public string Barcode { get => barcode; set => barcode = value; }
        public string ShowImg { get => showImg; set => showImg = value; }
        public string MainImg { get => mainImg; set => mainImg = value; }
        public string ImgUrl { get => imgUrl; set => imgUrl = value; }
        public string CostPrice { get => costPrice; set => costPrice = value; }
        public string OriginalPrice { get => originalPrice; set => originalPrice = value; }
        public string SalePrice { get => salePrice; set => salePrice = value; }
        public string VipPrice { get => vipPrice; set => vipPrice = value; }
        public string SpecData { get => specData; set => specData = value; }
        public string Unit { get => unit; set => unit = value; }
        public string InventoryNow { get => inventoryNow; set => inventoryNow = value; }
        public string InventoryMax { get => inventoryMax; set => inventoryMax = value; }
        public string InventoryMin { get => inventoryMin; set => inventoryMin = value; }
        public string IsDiscount { get => isDiscount; set => isDiscount = value; }
        public string IntegralGoods { get => integralGoods; set => integralGoods = value; }
        public string MakeDate { get => makeDate; set => makeDate = value; }
        public string ShelfLife { get => shelfLife; set => shelfLife = value; }
        public string Status { get => status; set => status = value; }
        public string ClassifyName { get => classifyName; set => classifyName = value; }
        public string Wholesale_price { get => wholesale_price; set => wholesale_price = value; }


        #region MyRegion
        //    /** 商店商品id */
        //private long goodsStoreId;

        //    /** 商品信息id */

        //private long goodsId;

        //    /** 分类id */

        //private long classifyId;

        //    /** 商家id */

        //private long shopId;

        //    /** 商品名称 */

        //private String goodsName;

        //    /** 拼音码 */

        //private String pinyinCode;

        //    /** 条码 */

        //private String barcode;

        //    /** 显示图片 */

        //private String showImg;

        //    /**商品主图*/
        //private String mainImg;

        //    /** 详情图片 */

        //private String imgUrl;

        //    /** 进货价 */

        //private double costPrice;

        //    /** 原价 */

        //private double originalPrice;

        //    /** 销售价 */

        //private double salePrice;

        //    /** 会员价 */

        //private double vipPrice;

        //    /** 规格 */

        //private String specData;

        //    /** 单位 */

        //private String unit;

        //    /** 当前库存 */

        //private String inventoryNow;

        //    /** 最大库存 */

        //private String inventoryMax;

        //    /** 最小库存 */

        //private String inventoryMin;

        //    /** 是否优惠（0是，1不是） */
        //private String isDiscount;

        //    /** 积分商品（0不是，1是） */

        //private String integralGoods;

        //    /** 生产日期 */

        //private DateTime makeDate;

        //    /** 保质期 */

        //private String shelfLife;

        //    /** 状态默认0（0=启用，1=未启用） */

        //private String status;

        //    /**  临时字段 分类名称  */
        //private String classifyName;

        //public long GoodsStoreId { get => goodsStoreId; set => goodsStoreId = value; }
        //public long GoodsId { get => goodsId; set => goodsId = value; }
        //public long ClassifyId { get => classifyId; set => classifyId = value; }
        //public long ShopId { get => shopId; set => shopId = value; }
        //public string GoodsName { get => goodsName; set => goodsName = value; }
        //public string PinyinCode { get => pinyinCode; set => pinyinCode = value; }
        //public string Barcode { get => barcode; set => barcode = value; }
        //public string ShowImg { get => showImg; set => showImg = value; }
        //public string MainImg { get => mainImg; set => mainImg = value; }
        //public string ImgUrl { get => imgUrl; set => imgUrl = value; }
        //public double CostPrice { get => costPrice; set => costPrice = value; }
        //public double OriginalPrice { get => originalPrice; set => originalPrice = value; }
        //public double SalePrice { get => salePrice; set => salePrice = value; }
        //public double VipPrice { get => vipPrice; set => vipPrice = value; }
        //public string SpecData { get => specData; set => specData = value; }
        //public string Unit { get => unit; set => unit = value; }
        //public string InventoryNow { get => inventoryNow; set => inventoryNow = value; }
        //public string InventoryMax { get => inventoryMax; set => inventoryMax = value; }
        //public string InventoryMin { get => inventoryMin; set => inventoryMin = value; }
        //public string IsDiscount { get => isDiscount; set => isDiscount = value; }
        //public string IntegralGoods { get => integralGoods; set => integralGoods = value; }
        //public DateTime MakeDate { get => makeDate; set => makeDate = value; }
        //public string ShelfLife { get => shelfLife; set => shelfLife = value; }
        //public string Status { get => status; set => status = value; }
        //public string ClassifyName { get => classifyName; set => classifyName = value; } 
        #endregion
    }
}
