using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using wordsnpages.Models;

namespace wordsnpages
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //keys of identity tables are mapped in the OnModelCreating
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id=1, Name="Action", DisplayOrder=1 },
                 new Category { Id=2, Name="Thriller", DisplayOrder=2 },
                  new Category { Id=3, Name="Romance", DisplayOrder=3 },
                   new Category { Id=4, Name="Mystery", DisplayOrder=4 }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Seat 7a\r\n\r\n",
                    Author="Sebastian Fitzek",
                    Description= "It's irrational.\r\n\r\nYou know that, you’ve checked the statistics. Flying is safer than driving — nineteen times safer.\r\n\r\nYou also know that should something happen, seat 7A, according to the numbers, is the worst place to be sitting.\r\n\r\nBut here you are, in seat 7A, buckled up for take off, unable to shake off the feeling of dread, the feeling that something is about to go very wrong.\r\n\r\nIrrational, perhaps.\r\n\r\nBut you're not wrong.\r\n ",
                    ISBN="SWD9999001",
                    ListPrice=99,
                    Price=90,
                    Price50=85,
                    Price100=80,
                    CategoryId =2,
                    ImageURL=""

                },
                new Product
                {
                    Id = 2,
                    Title = "BirdBox",
                    Author = "Michael Louis Calvillo",
                    Description = "Four young siblings. An ancient evil that has been waiting billions of years for its opportunity to reign over the earth. Oscar, Esteban, Manny and Isabel are as different as brothers and sisters can be. They argue, and the brothers even fight, but their unquestioned love for each other is about to be tested in unimaginable ways. The Birdbox has finally been opened after many centuries. Can they work together to overcome the darkness housed in The Birdbox in time to save themselves and their family? In time to save humanity itself?\r\n",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId =2,
                    ImageURL=""
                },
                new Product
                {
                    Id = 3,
                    Title = "The Hunger Games\r\n\r\n",
                    Author = "Suzanne Collins\r\n",
                    Description = "Could you survive on your own in the wild, with every one out to make sure you don't live to see the morning?\r\n\r\nIn the ruins of a place once known as North America lies the nation of Panem, a shining Capitol surrounded by twelve outlying districts. The Capitol is harsh and cruel and keeps the districts in line by forcing them all to send one boy and one girl between the ages of twelve and eighteen to participate in the annual Hunger Games, a fight to the death on live TV.\r\n\r\nSixteen-year-old Katniss Everdeen, who lives alone with her mother and younger sister, regards it as a death sentence when she steps forward to take her sister's place in the Games. But Katniss has been close to dead before—and survival, for her, is second nature. Without really meaning to, she becomes a contender. But if she is to win, she will have to start making choices that weight survival against humanity and life against love.\r\n",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId =1,
                    ImageURL=""
                },
                new Product
                {
                    Id = 4,
                    Title = "It Ends with Us\r\n\r\n",
                    Author = "Colleen Hoover",
                    Description = "Sometimes it is the one who loves you who hurts you the most.\r\n\r\nLily hasn’t always had it easy, but that’s never stopped her from working hard for the life she wants. She’s come a long way from the small town in Maine where she grew up — she graduated from college, moved to Boston, and started her own business. So when she feels a spark with a gorgeous neurosurgeon named Ryle Kincaid, everything in Lily’s life suddenly seems almost too good to be true.\r\n\r\nRyle is assertive, stubborn, maybe even a little arrogant. He’s also sensitive, brilliant, and has a total soft spot for Lily. And the way he looks in scrubs certainly doesn’t hurt. Lily can’t get him out of her head. But Ryle’s complete aversion to relationships is disturbing. Even as Lily finds herself becoming the exception to his “no dating” rule, she can’t help but wonder what made him that way in the first place.\r\n\r\nAs questions about her new relationship overwhelm her, so do thoughts of Atlas Corrigan — her first love and a link to the past she left behind. He was her kindred spirit, her protector. When Atlas suddenly reappears, everything Lily has built with Ryle is threatened.\r\n",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId =3,
                    ImageURL=""
                },
                new Product
                {
                    Id = 5,
                    Title = "It Starts With Us",
                    Author = "Colleen Hoover",
                    Description = " Before It Ends with Us, it started with Atlas. Colleen Hoover tells fan favorite Atlas’s side of the story and shares what comes next in this long-anticipated sequel to the “glorious and touching” (USA TODAY) #1 New York Times bestseller It Ends with Us.\r\n\r\nLily and her ex-husband, Ryle, have just settled into a civil coparenting rhythm when she suddenly bumps into her first love, Atlas, again. After nearly two years separated, she is elated that for once, time is on their side, and she immediately says yes when Atlas asks her on a date.\r\n\r\nBut her excitement is quickly hampered by the knowledge that, though they are no longer married, Ryle is still very much a part of her life—and Atlas Corrigan is the one man he will hate being in his ex-wife and daughter’s life.\r\n\r\nSwitching between the perspectives of Lily and Atlas, It Starts with Us picks up right where the epilogue for the “gripping, pulse-pounding” (Sarah Pekkanen, author of Perfect Neighbors) bestselling phenomenon It Ends with Us left off. Revealing more about Atlas’s past and following Lily as she embraces a second chance at true love while navigating a jealous ex-husband, it proves that “no one delivers an emotional read like Colleen Hoover” (Anna Todd, New York Times bestselling author).\r\n ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId =3,
                    ImageURL=""
                },
                new Product
                {
                    Id = 6,
                    Title = "The Clinic\r\n\r\n",
                    Author = "Cate Quinn",
                    Description = " From the critically acclaimed author of Black Widows comes a thriller set in a remote rehab clinic on the Pacific Northwest coast, in which the death of a woman inside prompts her sister to enter the clinic as a patient in order to find the truth. Perfect for fans of Stacy Willingham and Tarryn Fisher!\r\n\r\nMeg works for a casino in LA, catching cheaters and popping a few too many pain pills to cope, following a far different path than her sister Haley, a famous actress. But suddenly reports surface of Haley dying at the remote rehab facility where she had been forced to go to get her addictions under control.\r\n\r\nThere are whispers of suicide, but Meg can't believe it. She decides that the best way to find out what happened to her sister is to check in herself – to investigate what really happened from the inside.\r\n\r\nBattling her own addictions and figuring out the truth will be much more difficult than she imagined, far away from friends, family – and anyone who could help her.\r\n ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId=4,
                    ImageURL=""

                },
                 new Product
                 {
                     Id = 7,
                     Title = "The Clinic\r\n\r\n",
                     Author = "Cate Quinn",
                     Description = " From the critically acclaimed author of Black Widows comes a thriller set in a remote rehab clinic on the Pacific Northwest coast, in which the death of a woman inside prompts her sister to enter the clinic as a patient in order to find the truth. Perfect for fans of Stacy Willingham and Tarryn Fisher!\r\n\r\nMeg works for a casino in LA, catching cheaters and popping a few too many pain pills to cope, following a far different path than her sister Haley, a famous actress. But suddenly reports surface of Haley dying at the remote rehab facility where she had been forced to go to get her addictions under control.\r\n\r\nThere are whispers of suicide, but Meg can't believe it. She decides that the best way to find out what happened to her sister is to check in herself – to investigate what really happened from the inside.\r\n\r\nBattling her own addictions and figuring out the truth will be much more difficult than she imagined, far away from friends, family – and anyone who could help her.\r\n ",
                     ISBN = "FOT000000001",
                     ListPrice = 25,
                     Price = 23,
                     Price50 = 22,
                     Price100 = 20,
                     CategoryId=4,
                     ImageURL=""

                 }
                );
            modelBuilder.Entity<Company>().HasData(
                new Company { Id=1, Name="Tech Solution", StreetAddress="123 Main St", City="Main", PostalCode="12121", State="IL", PhoneNumber="123456789" }
                );
        }
    }
}
