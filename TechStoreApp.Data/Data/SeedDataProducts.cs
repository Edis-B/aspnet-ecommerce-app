using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Data
{
    public static class SeedDataProducts
    {
        public static List<Product> GetProducts()
        {
            var products = new List<Product>();

            // Configured Systems
            products.AddRange(new List<Product>
            {
                new Product
                {
                    IsFeatured = true,
                    CategoryId = 1, // Assume category ID exists
                    Name = "Gaming PC Starter Pack",
                    Description = "A powerful gaming PC setup with high-performance specs.",
                    Price = 999.99m,
                    Stock = 10,
                    ImageUrl = "https://ucarecdn.com/a096ad52-c971-4447-87c9-866eb47bffa9/FOCUSEXTREME.png"
                },
                new Product
                {
                    CategoryId = 1,
                    Name = "Office Workstation Pro",
                    Description = "A reliable workstation for all your office needs.",
                    Price = 799.99m,
                    Stock = 15,
                    ImageUrl = "https://www.vali.bg/UserFiles/Product/gallery_1/1-1-6fb3f5cc-5f6c-e4f4-cc4a-23362c531cc0.jpg?w=640&h=640&block&cache"
                },
                new Product
                {
                    CategoryId = 1,
                    Name = "Creative Studio System",
                    Description = "Designed for artists and creators with high RAM and GPU.",
                    Price = 1299.99m,
                    Stock = 8,
                    ImageUrl = "https://plecom.imgix.net/iil-351033-658083.jpg?fit=fillmax&fill=solid&fill-color=ffffff&auto=format&w=1000&h=1000"
                },
                new Product
                {
                    CategoryId = 1,
                    Name = "Compact Mini PC",
                    Description = "Space-saving mini PC for basic tasks and entertainment.",
                    Price = 499.99m,
                    Stock = 12,
                    ImageUrl = "https://www.jouleperformance.com/media/catalog/product/4/2/420a5a4f-1382-4752-825f-6ab7debd3827.png?optimize=medium&fit=bounds&height=230&width=230&canvas=230:230"
                },
                new Product
                {
                    CategoryId = 1,
                    Name = "High-Performance Gaming Rig",
                    Description = "Top-tier gaming rig designed for VR and high settings.",
                    Price = 1999.99m,
                    Stock = 5,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSHurUzdR2xzTAsQ9NgeWFvxUoo59G7bDBs4A&s"
                },
                new Product
                {
                    IsFeatured = true,
                    CategoryId = 2,
                    Name = "NVIDIA GeForce RTX 3080",
                    Description = "High-end graphics card for gamers and creators.",
                    Price = 699.99m,
                    Stock = 5,
                    ImageUrl = "https://static.gigabyte.com/StaticFile/Image/Global/1f7a4b7372688a9959a997aa486252e1/Product/25956/Png"
                },
                new Product
                {
                    CategoryId = 2,
                    Name = "AMD Radeon RX 6800 XT",
                    Description = "Top-tier graphics card with exceptional performance.",
                    Price = 649.99m,
                    Stock = 6,
                    ImageUrl = "https://pcbuild.bg/assets/products/000/000/092/000000092479--video-karta-msi-radeon-rx-6800-xt-gaming-x-trio-16g.jpg"
                },
                new Product
                {
                    CategoryId = 2,
                    Name = "NVIDIA GeForce GTX 1660",
                    Description = "Affordable graphics card for casual gaming.",
                    Price = 249.99m,
                    Stock = 20,
                    ImageUrl = "https://static.gigabyte.com/StaticFile/Image/Global/02fefc0df77c561e3ebcdb8ebbec5b0a/Product/22932/Png"
                },
                new Product
                {
                    CategoryId = 2,
                    Name = "ASUS ROG Strix GeForce RTX 3060",
                    Description = "Efficient graphics card for smooth 1080p gaming.",
                    Price = 399.99m,
                    Stock = 10,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTYadv_n09Tj8mFBkfQeXIZ5TpyHN4mtkHu_Q&s"
                },
                new Product
                {
                    CategoryId = 2,
                    Name = "MSI Radeon RX 5700 XT",
                    Description = "High-performance graphics card for serious gamers.",
                    Price = 499.99m,
                    Stock = 8,
                    ImageUrl = "https://p.jarcomputers.com/680x680/8f/VCRMSIRADEONRX5700XTMECHOC_680x680.jpg"
                },
                new Product
                {
                    IsFeatured = true,
                    CategoryId = 3,
                    Name = "Intel Core i9-11900K",
                    Description = "Top-of-the-line CPU for extreme gaming performance.",
                    Price = 599.99m,
                    Stock = 7,
                    ImageUrl = "https://www.sense.lk/images/uploads/product/2024/04/20240402131528i9.png"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "AMD Ryzen 9 5900X",
                    Description = "High-performance CPU with 12 cores for multitasking.",
                    Price = 549.99m,
                    Stock = 10,
                    ImageUrl = "https://gfx3.senetic.com/akeneo-catalog/2/4/8/2/248285e0a754b5b0e38e8d5cb19fc7a194c6bdcf_1661519_100_100000061WOF_image1.jpg"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "Intel Core i5-11600K",
                    Description = "Mid-range CPU for budget gamers and content creators.",
                    Price = 299.99m,
                    Stock = 15,
                    ImageUrl = "https://s13emagst.akamaized.net/products/36114/36113437/images/res_21a441ab897cb64e3d7285cbd243d8d9.jpg"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "AMD Ryzen 7 5800X",
                    Description = "Great for gaming and productivity with 8 cores.",
                    Price = 449.99m,
                    Stock = 9,
                    ImageUrl = "https://pcbuild.bg/assets/products/000/000/081/000000081774.jpg"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "Intel Core i3-10100",
                    Description = "Budget-friendly CPU for basic tasks.",
                    Price = 139.99m,
                    Stock = 20,
                    ImageUrl = "https://gplay.bg/UserFiles/Product/gallery_1/af669eb8-9eaf-cb06-56ce-507d8db15de5.jpg"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "AMD Ryzen 5 5600X",
                    Description = "Affordable processor with excellent performance for gaming.",
                    Price = 259.99m,
                    Stock = 12,
                    ImageUrl = "https://ardes.bg/uploads/original/amd-cpu-desktop-ryzen-5-6c-12t-5600x-3-7-4-6ghz-ma-294174.jpg"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "Intel Core i5-10400",
                    Description = "Solid mid-range processor for everyday use.",
                    Price = 189.99m,
                    Stock = 18,
                    ImageUrl = "https://ardes.bg/uploads/original/intel-cpu-desktop-core-i5-10400-2-9ghz-12mb-lga120-275671.jpg"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "Intel Xeon E-2236",
                    Description = "Server-grade processor with high reliability.",
                    Price = 399.99m,
                    Stock = 5,
                    ImageUrl = "https://m.media-amazon.com/images/I/41MOzhSuHyL._AC_UF894,1000_QL80_.jpg"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "AMD Athlon 3000G",
                    Description = "Budget processor for basic computing needs.",
                    Price = 49.99m,
                    Stock = 25,
                    ImageUrl = "https://ardes.bg/uploads/original/protsesor-amd-athlon-3000g-2-core-3-5-ghz-5mb-35w-257581.jpg"
                },
                new Product
                {
                    CategoryId = 3,
                    Name = "Intel Core i7-11700K",
                    Description = "High-performance CPU for gaming and content creation.",
                    Price = 399.99m,
                    Stock = 8,
                    ImageUrl = "https://s13emagst.akamaized.net/products/36114/36113436/images/res_4a5bd4d0e179dee4228b716160777993.jpg"
                },
                new Product
                {
                    CategoryId = 4,
                    Name = "Cooler Master Hyper 212",
                    Description = "High-performance air cooler for CPUs.",
                    Price = 39.99m,
                    Stock = 25,
                    ImageUrl = "https://gplay.bg/UserFiles/Product/gallery_1/hyper-212-black-x-duo-gallery-02-image--custom-.png"
                },
                new Product
                {
                    CategoryId = 4,
                    Name = "NZXT Kraken X63",
                    Description = "All-in-one liquid cooler for maximum performance.",
                    Price = 129.99m,
                    Stock = 10,
                    ImageUrl = "https://thx.bg/UserFiles/Product/gallery_1/499e3ef7-a4ed-5d0a-f05f-e59dd558dd08.png?w=1000&h=1000&cache&block"
                },
                new Product
                {
                    CategoryId = 4,
                    Name = "be quiet! Dark Rock 4",
                    Description = "Silent and efficient air cooler for enthusiasts.",
                    Price = 89.99m,
                    Stock = 15,
                    ImageUrl = "https://bestpc.bg/images/productimages-normal/378870.jpg"
                },
                new Product
                {
                    CategoryId = 4,
                    Name = "Noctua NH-D15",
                    Description = "Premium air cooler known for its exceptional performance.",
                    Price = 99.99m,
                    Stock = 8,
                    ImageUrl = "https://gplay.bg/UserFiles/Product/gallery_1/nh-d15-g2-1-1-1.jpg?block&cache&w=1000&h=1000"
                },
                new Product
                {
                    CategoryId = 4,
                    Name = "Thermaltake Water 3.0",
                    Description = "Reliable liquid cooling solution for high-performance CPUs.",
                    Price = 89.99m,
                    Stock = 12,
                    ImageUrl = "https://www.thermaltake.com/media/catalog/product/cache/77748820fd1566e3ca73f32bfa96ba8f/db/imgs/pdt/angle/CL-W007-PL12BL-A_e3ab6c490af64a869421d8db86bfe896.jpg"
                },
                new Product
                {
                    CategoryId = 5,
                    Name = "NZXT H510",
                    Description = "Mid-tower case with excellent airflow and cable management.",
                    Price = 69.99m,
                    Stock = 20,
                    ImageUrl = "https://pcbuild.bg/assets/products/000/000/209/000000209955-listing--kutiya-nzxt-h510-flow-matte-white.webp"
                },
                new Product
                {
                    CategoryId = 5,
                    Name = "Fractal Design Meshify C",
                    Description = "Compact case with mesh front for optimal cooling.",
                    Price = 89.99m,
                    Stock = 15,
                    ImageUrl = "https://www.fractal-design.com/app/uploads/2019/06/Meshify-C_1.jpg"
                },
                new Product
                {
                    CategoryId = 5,
                    Name = "Corsair 4000D Airflow",
                    Description = "High airflow case with spacious interior.",
                    Price = 79.99m,
                    Stock = 10,
                    ImageUrl = "https://www.computermarket.bg/media/catalog/product/4/c/4c2d706d-9d97-fde9-484d-6d4befb72f31.jpeg"
                },
                new Product
                {
                    CategoryId = 5,
                    Name = "Thermaltake View 71",
                    Description = "Spacious case with tempered glass for showcasing builds.",
                    Price = 149.99m,
                    Stock = 5,
                    ImageUrl = "https://gplay.bg/UserFiles/Product/gallery_1/e10da2d7-9d36-8537-3165-c262b7793c4ar.jpg"
                },
                new Product
                {
                    CategoryId = 5,
                    Name = "Cooler Master MasterBox Q300L",
                    Description = "Compact case perfect for small form factor builds.",
                    Price = 59.99m,
                    Stock = 12,
                    ImageUrl = "https://files.coolermaster.com/og-image/masterbox-q300l-1200x630.jpg"
                },
                new Product
                {
                    CategoryId = 6,
                    Name = "ASUS ROG Strix B550-F",
                    Description = "High-performance motherboard for AMD Ryzen CPUs.",
                    Price = 189.99m,
                    Stock = 10,
                    ImageUrl = "https://desktop.bg/system/images/297647/original/asus_rog_strix_b550f_gaming.png"
                },
                new Product
                {
                    CategoryId = 6,
                    Name = "MSI MPG Z490 Gaming Edge WiFi",
                    Description = "Feature-rich motherboard for Intel 10th gen CPUs.",
                    Price = 199.99m,
                    Stock = 8,
                    ImageUrl = "https://asset.msi.com/resize/image/global/product/product_160938693173d821d3e96c87e573f494cb2197728e.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png"
                },
                new Product
                {
                    CategoryId = 6,
                    Name = "Gigabyte B550 AORUS Elite",
                    Description = "Solid motherboard with great performance for gamers.",
                    Price = 159.99m,
                    Stock = 12,
                    ImageUrl = "https://desktop.bg/system/images/327874/double/gigabyte_b550m_aorus_elite_v2.png"
                },
                new Product
                {
                    CategoryId = 6,
                    Name = "ASRock B450M Pro4",
                    Description = "Affordable motherboard for budget builds.",
                    Price = 89.99m,
                    Stock = 15,
                    ImageUrl = "https://www.asrock.com/mb/photo/B450M%20Pro4%20R2.0(M1).png"
                },
                new Product
                {
                    CategoryId = 6,
                    Name = "ASUS Prime X570-Pro",
                    Description = "High-end motherboard with excellent overclocking features.",
                    Price = 249.99m,
                    Stock = 6,
                    ImageUrl = "https://s9.vivre.eu/upload/2023/08/thumbs/33949894-ca19648e-1a52-47e8-9b3d-4d807ebb0fe7.330x9999.jpg"
                },
                new Product
                {
                    CategoryId = 7,
                    Name = "Corsair Vengeance LPX 16GB",
                    Description = "High-performance RAM for gamers and content creators.",
                    Price = 89.99m,
                    Stock = 20,
                    ImageUrl = "https://desktop.bg/system/images/335375/normal/16gb_2x8gb_ddr4_3200mhz_corsair_vengeance_lpx_black_duplicate.png"
                },
                new Product
                {
                    CategoryId = 7,
                    Name = "G.Skill Ripjaws V 32GB",
                    Description = "Ideal RAM for content creation and multitasking.",
                    Price = 139.99m,
                    Stock = 10,
                    ImageUrl = "https://s13emagst.akamaized.net/products/3951/3950379/images/res_2014604ed34570f6579b45171ba502e7.jpg"
                },
                new Product
                {

                    CategoryId = 7,
                    Name = "Kingston Fury Beast 16GB",
                    Description = "Reliable RAM for budget-friendly builds.",
                    Price = 79.99m,
                    Stock = 20,
                    ImageUrl = "https://desktop.bg/system/images/335375/normal/16gb_2x8gb_ddr4_3200mhz_corsair_vengeance_lpx_black_duplicate.png"
                },
                new Product
                {
                    CategoryId = 7,
                    Name = "HyperX Fury 2x16GB",
                    Description = "High-capacity RAM for serious gamers.",
                    Price = 129.99m,
                    Stock = 15,
                    ImageUrl = "https://img-cdn.heureka.group/v1/460600f3-eecd-48ce-bf1a-96650c07d95d.jpg?width=400&height=400"
                },
                new Product
                {
                    CategoryId = 7,
                    Name = "Crucial Ballistix 2x8GB",
                    Description = "Excellent RAM for high-performance builds.",
                    Price = 89.99m,
                    Stock = 18,
                    ImageUrl = "https://desktop.bg/system/images/247997/double/16gb_2x8gb_ddr4_3200mhz_crucial_ballistix_rgb.png"
                },
                new Product
                {
                    CategoryId = 8,
                    Name = "Samsung 970 EVO Plus 1TB",
                    Description = "Fast NVMe SSD for superior performance.",
                    Price = 169.99m,
                    Stock = 15,
                    ImageUrl = "https://ardes.bg/uploads/original/enterprise-ssd-samsung-970-evo-plus-series-1-tb-3d-222588.jpg"
                },
                new Product
                {
                    CategoryId = 8,
                    Name = "Crucial MX500 500GB",
                    Description = "Reliable SATA SSD with good performance.",
                    Price = 59.99m,
                    Stock = 25,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTksrxaphvsrJfVe-_p8aPNazzlv07leiAJQw&s"
                },
                new Product
                {
                    CategoryId = 8,
                    Name = "Western Digital Blue SN550 1TB",
                    Description = "Affordable NVMe SSD with solid performance.",
                    Price = 109.99m,
                    Stock = 12,
                    ImageUrl = "https://m.media-amazon.com/images/I/61D3J+ZQUAL._AC_UF1000,1000_QL80_.jpg"
                },
                new Product
                {
                    CategoryId = 8,
                    Name = "Kingston A2000 500GB",
                    Description = "Budget NVMe SSD with great value.",
                    Price = 59.99m,
                    Stock = 20,
                    ImageUrl = "https://ardes.bg/uploads/original/500gb-ssd-kingston-a2000-sin-244907.jpg"
                },
                new Product
                {
                    CategoryId = 8,
                    Name = "ADATA XPG SX8200 Pro 1TB",
                    Description = "High-speed SSD for gamers and professionals.",
                    Price = 129.99m,
                    Stock = 10,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ8Z5Yv2qlgYNSts8GmWCRKHwumLSujclFHzw&s"
                },
                new Product
                {
                    CategoryId = 9,
                    Name = "Seagate Barracuda 2TB",
                    Description = "Reliable hard drive for data storage.",
                    Price = 54.99m,
                    Stock = 30,
                    ImageUrl = "https://img-cdn.heureka.group/v1/56ba896a-cae6-41e8-abdd-e327724734df.jpg?width=350&height=350"
                },
                new Product
                {
                    CategoryId = 9,
                    Name = "Western Digital Blue 1TB",
                    Description = "Affordable hard disk drive with good performance.",
                    Price = 49.99m,
                    Stock = 20,
                    ImageUrl = "https://thx.bg/UserFiles/Product/gallery_1/1_wd_blue_1.png"
                },
                new Product
                {
                    CategoryId = 9,
                    Name = "Toshiba X300 4TB",
                    Description = "High-capacity hard drive for massive storage needs.",
                    Price = 99.99m,
                    Stock = 10,
                    ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/712CNU+QimL._MCnd_AC_SR462,462_.jpg"
                },
                new Product
                {
                    CategoryId = 9,
                    Name = "HGST Deskstar 5TB",
                    Description = "Reliable storage for large data sets.",
                    Price = 129.99m,
                    Stock = 8,
                    ImageUrl = "https://www.bhphotovideo.com/images/images500x500/hgst_0s03835_5tb_deckstar_sata_3_5_1417347905_1102471.jpg"
                },
                new Product
                {
                    CategoryId = 9,
                    Name = "Seagate IronWolf 8TB",
                    Description = "Optimized for NAS systems and large data storage.",
                    Price = 179.99m,
                    Stock = 5,
                    ImageUrl = "https://gplay.bg/UserFiles/Product/gallery_1/seagate-st8000nt001-1.jpg?block&cache&w=1000&h=1000"
                },
                new Product
                {
                    CategoryId = 10,
                    Name = "Corsair RM750x 750W",
                    Description = "High-quality power supply with modular design.",
                    Price = 129.99m,
                    Stock = 15,
                    ImageUrl = "https://assets.corsair.com/image/upload/f_auto,q_auto/content/CP-9020179-NA-RM750x-PSU-01.png"
                },
                new Product
                {
                    CategoryId = 10,
                    Name = "EVGA 600 W1 600W",
                    Description = "Reliable power supply for budget builds.",
                    Price = 49.99m,
                    Stock = 25,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBg19fTsd4oeWMFnFEsi20ZaHZFy15XKHz8g&s"
                },
                new Product
                {
                    CategoryId = 10,
                    Name = "Seasonic Focus GX-850 850W",
                    Description = "Premium power supply with high efficiency.",
                    Price = 139.99m,
                    Stock = 10,
                    ImageUrl = "https://www.dekada.com/images/seasonic_focus_gx_850_0_box_1_Dki20l.jpg"
                },
                new Product
                {
                    CategoryId = 10,
                    Name = "Thermaltake Toughpower GF1 750W",
                    Description = "Power supply with excellent build quality.",
                    Price = 99.99m,
                    Stock = 12,
                    ImageUrl = "https://desktop.bg/system/images/462325/original/thermaltake_toughpower_gf1_750w_tt_premium_edition.jpg"
                },
                new Product
                {
                    CategoryId = 10,
                    Name = "Cooler Master MWE Gold 650W",
                    Description = "Gold-rated power supply for high-performance systems.",
                    Price = 89.99m,
                    Stock = 10,
                    ImageUrl = "https://www.vario.bg/images/product/36213/MPE-6501-AFAAG-EU.jpg"
                }
            });

            return products;
        }
    }
}
