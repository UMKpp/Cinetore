using Microsoft.EntityFrameworkCore;
using MovieStore.Models;
using System.Text.Json;

namespace MovieStore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MovieStoreContext>();

            // Ensure database is created and up to date
            context.Database.Migrate();

            // Enrich legacy records if present
            var titanic = context.Movies.FirstOrDefault(m => m.Title == "Titanic");
            if (titanic != null && string.IsNullOrEmpty(titanic.PosterUrl))
            {
                titanic.ReleaseYear ??= 1997;
                titanic.Director ??= "James Cameron";
                titanic.Rating ??= "7.9 / 10";
                titanic.PosterUrl = "https://upload.wikimedia.org/wikipedia/en/1/18/Titanic_%281997_film%29_poster.png";
                if (string.IsNullOrWhiteSpace(titanic.Description))
                {
                    titanic.Description = "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic.";
                }
            }

            var inception = context.Movies.FirstOrDefault(m => m.Title == "Inception");
            if (inception != null && string.IsNullOrEmpty(inception.PosterUrl))
            {
                inception.ReleaseYear ??= 2010;
                inception.Director ??= "Christopher Nolan";
                inception.Rating ??= "8.8 / 10";
                inception.PosterUrl = "https://upload.wikimedia.org/wikipedia/en/2/2e/Inception_%282010%29_theatrical_poster.jpg";
                if (string.IsNullOrWhiteSpace(inception.Description))
                {
                    inception.Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.";
                }
                inception.Cast = JsonSerializer.Serialize(new[]
                {
                    new CastMember { Name = "Leonardo DiCaprio", Role = "Dom Cobb" }
                });
            }

            var hp1 = context.Movies.FirstOrDefault(m => m.Title == "Harry Potter and the Sorcerer's Stone");
            if (hp1 != null && string.IsNullOrEmpty(hp1.PosterUrl))
            {
                hp1.ReleaseYear ??= 2001;
                hp1.Director ??= "Chris Columbus";
                hp1.Rating ??= "7.6 / 10";
                hp1.PosterUrl = "https://upload.wikimedia.org/wikipedia/en/7/7a/Harry_Potter_and_the_Philosopher%27s_Stone_banner.jpg";
                if (string.IsNullOrWhiteSpace(hp1.Description))
                {
                    hp1.Description = "An orphaned boy enrolls in a school of wizardry, where he learns the truth about himself, his family and the terrible evil that haunts the magical world.";
                }
            }

            var hp2 = context.Movies.FirstOrDefault(m => m.Title == "Harry Potter and Chamber of the Secrets");
            if (hp2 != null && string.IsNullOrEmpty(hp2.PosterUrl))
            {
                hp2.ReleaseYear ??= 2002;
                hp2.Director ??= "Chris Columbus";
                hp2.Rating ??= "7.4 / 10";
                hp2.PosterUrl = "https://upload.wikimedia.org/wikipedia/en/c/c0/Harry_Potter_and_the_Chamber_of_Secrets_movie_poster.jpg";
                if (string.IsNullOrWhiteSpace(hp2.Description))
                {
                    hp2.Description = "An ancient prophecy seems to be coming true when a mysterious presence begins stalking the corridors of a school of magic.";
                }
            }

            // Original 16 Movies from frontend catalog
            var originalCatalog = new List<Movie>
            {
                new Movie
                {
                    Title = "Spider-Man: Brand New Day",
                    Genre = "Action",
                    ReleaseYear = 2026,
                    Rating = "N/A / 10",
                    Director = "Destin Daniel Cretton",
                    Price = 19.99m,
                    PosterUrl = "https://media.themoviedb.org/t/p/w220_and_h330_face/bjiS5ipwxb9JFy3XRRN4OAilSeX.jpg",
                    Description = "Peter Parker enters a new chapter as Spider-Man, balancing the demands of protecting New York with the uncertainty of what his future holds. The upcoming Marvel adventure brings Tom Holland back to the role for another web-slinging story directed by Destin Daniel Cretton.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Tom Holland", Role = "Peter Parker / Spider-Man" },
                        new CastMember { Name = "Zendaya", Role = "Michelle Jones-Watson" },
                        new CastMember { Name = "Jacob Batalon", Role = "Ned Leeds" },
                        new CastMember { Name = "Sadie Sink", Role = "Cast role undisclosed" }
                    })
                },
                new Movie
                {
                    Title = "Red Notice",
                    Genre = "Action",
                    ReleaseYear = 2021,
                    Rating = "6.3 / 10",
                    Director = "Rawson Marshall Thurber",
                    Price = 14.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/1/1b/SAS_Red_Notice_film_poster.png",
                    Description = "An FBI profiler is forced into an uneasy alliance with the world's most accomplished art thief when a daring rival threatens an international treasure hunt. The chase moves from one glamorous setting to another, powered by shifting loyalties, elaborate scams, and three sharply contrasting personalities.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Dwayne Johnson", Role = "John Hartley" },
                        new CastMember { Name = "Gal Gadot", Role = "The Bishop" },
                        new CastMember { Name = "Ryan Reynolds", Role = "Nolan Booth" }
                    })
                },
                new Movie
                {
                    Title = "Extraction",
                    Genre = "Action",
                    ReleaseYear = 2020,
                    Rating = "6.7 / 10",
                    Director = "Sam Hargrave",
                    Price = 12.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/1/1a/Extraction_Movie_Poster.jpg",
                    Description = "Black-market mercenary Tyler Rake is sent into Dhaka to rescue Ovi, the kidnapped son of an imprisoned crime lord. As the operation spirals through crowded streets and hostile territory, Tyler must protect his young charge while confronting the choices that shaped his own damaged past.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Chris Hemsworth", Role = "Tyler Rake" },
                        new CastMember { Name = "Rudhraksh Jaiswal", Role = "Ovi Mahajan" },
                        new CastMember { Name = "Randeep Hooda", Role = "Saju Rav" },
                        new CastMember { Name = "Golshifteh Farahani", Role = "Nik Khan" }
                    })
                },
                new Movie
                {
                    Title = "The Gray Man",
                    Genre = "Action",
                    ReleaseYear = 2022,
                    Rating = "6.5 / 10",
                    Director = "Anthony and Joe Russo",
                    Price = 14.99m,
                    PosterUrl = "https://image.tmdb.org/t/p/w500/8cXbitsS6dWQ5gfMTZdorpAAzEH.jpg",
                    Description = "When a highly skilled CIA operative uncovers secrets hidden inside the agency, he becomes the target of a relentless former colleague. Pursued across Europe by an international team, he must use improvisation and uneasy alliances to stay alive and protect the people caught in the hunt.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Ryan Gosling", Role = "Court Gentry / Six" },
                        new CastMember { Name = "Chris Evans", Role = "Lloyd Hansen" },
                        new CastMember { Name = "Ana de Armas", Role = "Dani Miranda" },
                        new CastMember { Name = "Regé-Jean Page", Role = "Denny Carmichael" }
                    })
                },
                new Movie
                {
                    Title = "Don't Look Up",
                    Genre = "Sci-Fi",
                    ReleaseYear = 2021,
                    Rating = "7.1 / 10",
                    Director = "Adam McKay",
                    Price = 12.99m,
                    PosterUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQzS__a_h04jU5rF3d0bGggLBpjqnYvr7L3Z67RvuB1GA&s=10",
                    Description = "When astronomy graduate student Kate Dibiasky and her professor, Randall Mindy, discover a comet on a collision course with Earth, they launch an urgent media tour to warn the public. Their attempt to make a world-changing danger impossible to ignore becomes a sharp, funny look at celebrity culture, politics, and denial.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Leonardo DiCaprio", Role = "Dr. Randall Mindy" },
                        new CastMember { Name = "Jennifer Lawrence", Role = "Kate Dibiasky" },
                        new CastMember { Name = "Meryl Streep", Role = "President Orlean" },
                        new CastMember { Name = "Jonah Hill", Role = "Jason Orlean" }
                    })
                },
                new Movie
                {
                    Title = "The Adam Project",
                    Genre = "Sci-Fi",
                    ReleaseYear = 2022,
                    Rating = "6.7 / 10",
                    Director = "Shawn Levy",
                    Price = 14.99m,
                    PosterUrl = "https://image.tmdb.org/t/p/w500/wFjboE0aFZNbVOF05fzrka9Fqyx.jpg",
                    Description = "After an accidental landing in the past, time-traveling pilot Adam Reed meets his younger self and joins forces with the boy to find the truth behind his mission. Their adventure mixes high-stakes science fiction with a heartfelt story about family, grief, and making peace with the person you used to be.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Ryan Reynolds", Role = "Adam Reed" },
                        new CastMember { Name = "Walker Scobell", Role = "Young Adam Reed" },
                        new CastMember { Name = "Jennifer Garner", Role = "Ellie Reed" },
                        new CastMember { Name = "Mark Ruffalo", Role = "Louis Reed" }
                    })
                },
                new Movie
                {
                    Title = "They Cloned Tyrone",
                    Genre = "Sci-Fi",
                    ReleaseYear = 2023,
                    Rating = "6.6 / 10",
                    Director = "Juel Taylor",
                    Price = 11.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/5/53/They_cloned_tyrone_poster.png",
                    Description = "A streetwise trio in a mysterious housing complex begin noticing strange events that do not add up. Their investigation leads them through a pulpy, funny science-fiction conspiracy involving surveillance, identity, and the forces shaping their neighborhood.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "John Boyega", Role = "Fontaine" },
                        new CastMember { Name = "Teyonah Parris", Role = "Yo-Yo" },
                        new CastMember { Name = "Jamie Foxx", Role = "Slick Charles" },
                        new CastMember { Name = "Kiefer Sutherland", Role = "Nixon" }
                    })
                },
                new Movie
                {
                    Title = "Marriage Story",
                    Genre = "Drama",
                    ReleaseYear = 2019,
                    Rating = "7.9 / 10",
                    Director = "Noah Baumbach",
                    Price = 12.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/f/f4/Marriage_Story.jpg",
                    Description = "A theater director and an actor begin the difficult process of ending their marriage while trying to remain present for their young son. Moving between New York and Los Angeles, the film observes the tenderness, frustration, and complicated negotiations that remain after love changes shape.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Scarlett Johansson", Role = "Nicole Barber" },
                        new CastMember { Name = "Adam Driver", Role = "Charlie Barber" },
                        new CastMember { Name = "Laura Dern", Role = "Nora Fanshaw" },
                        new CastMember { Name = "Ray Liotta", Role = "Jay Marotta" }
                    })
                },
                new Movie
                {
                    Title = "The Irishman",
                    Genre = "Drama",
                    ReleaseYear = 2019,
                    Rating = "7.8 / 10",
                    Director = "Martin Scorsese",
                    Price = 14.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/8/80/The_Irishman_poster.jpg",
                    Description = "A former truck driver reflects on the decades he spent working as a fixer for organized crime and on the powerful union leader Jimmy Hoffa who became his friend. The sweeping crime drama moves through memory, loyalty, and regret as one man's life is measured against the choices he made.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Robert De Niro", Role = "Frank Sheeran" },
                        new CastMember { Name = "Al Pacino", Role = "Jimmy Hoffa" },
                        new CastMember { Name = "Joe Pesci", Role = "Russell Bufalino" },
                        new CastMember { Name = "Anna Paquin", Role = "Peggy Sheeran" }
                    })
                },
                new Movie
                {
                    Title = "Society of the Snow",
                    Genre = "Drama",
                    ReleaseYear = 2023,
                    Rating = "7.8 / 10",
                    Director = "J. A. Bayona",
                    Price = 14.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/7/76/Society_of_the_Snow_poster.jpg",
                    Description = "After a plane carrying a Uruguayan rugby team crashes in the Andes, the survivors face an unforgiving landscape with little food, warmth, or hope of immediate rescue. The story follows their shared courage and difficult decisions as they rely on one another to endure an impossible ordeal.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Enzo Vogrincic", Role = "Numa Turcatti" },
                        new CastMember { Name = "Agustín Pardella", Role = "Nando Parrado" },
                        new CastMember { Name = "Matías Recalt", Role = "Roberto Canessa" },
                        new CastMember { Name = "Diego Vegezzi", Role = "Marcelo Pérez" }
                    })
                },
                new Movie
                {
                    Title = "When Harry Met Sally",
                    Genre = "RomCom",
                    ReleaseYear = 1989,
                    Rating = "7.7 / 10",
                    Director = "Rob Reiner",
                    Price = 9.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/1/1c/WhenHarryMetSallyPoster.jpg",
                    Description = "Harry Burns and Sally Albright meet during a cross-country drive and keep finding their way back into each other's lives over the years. Their conversations about friendship, love, and timing gradually reveal a relationship that is more complicated and more lasting than either expects.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Billy Crystal", Role = "Harry Burns" },
                        new CastMember { Name = "Meg Ryan", Role = "Sally Albright" },
                        new CastMember { Name = "Carrie Fisher", Role = "Marie" },
                        new CastMember { Name = "Bruno Kirby", Role = "Jess" }
                    })
                },
                new Movie
                {
                    Title = "Sweet Home Alabama",
                    Genre = "RomCom",
                    ReleaseYear = 2002,
                    Rating = "6.2 / 10",
                    Director = "Andy Tennant",
                    Price = 9.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/f/fc/Sweet_Home_Alabama_film.jpg",
                    Description = "New York fashion designer Melanie Carmichael appears to have built a perfect new life, but a surprise engagement forces her to return to Alabama and settle unfinished business. Back home, old relationships and new ambitions make her reconsider the story she has been telling about where she belongs.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Reese Witherspoon", Role = "Melanie Carmichael" },
                        new CastMember { Name = "Josh Lucas", Role = "Jake Perry" },
                        new CastMember { Name = "Patrick Dempsey", Role = "Andrew Hennings" },
                        new CastMember { Name = "Candice Bergen", Role = "Kate Hennings" }
                    })
                },
                new Movie
                {
                    Title = "The Love Hypothesis",
                    Genre = "RomCom",
                    ReleaseYear = 2026,
                    Rating = "N/A / 10",
                    Director = "Brett Haley",
                    Price = 16.99m,
                    PosterUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT-OxBAioogfs5l3mKxzJpkNodxDknnddccmIGdrg2JvHvVRPfT-mB0clA&s=10",
                    Description = "Biology researcher Olive Smith agrees to a fake relationship with intimidating professor Adam Carlsen, hoping to simplify her personal life while keeping her academic ambitions on track. Their carefully arranged pretend romance grows more complicated as professional rivalry gives way to unexpected understanding.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Daisy Edgar-Jones", Role = "Olive Smith" },
                        new CastMember { Name = "Ryan Gosling", Role = "Adam Carlsen" }
                    })
                },
                new Movie
                {
                    Title = "Spirited Away",
                    Genre = "Animation",
                    ReleaseYear = 2001,
                    Rating = "8.6 / 10",
                    Director = "Hayao Miyazaki",
                    Price = 14.99m,
                    PosterUrl = "https://image.tmdb.org/t/p/w500/39wmItIWsg5sZMyRUHLkWBcuVCM.jpg",
                    Description = "Ten-year-old Chihiro becomes trapped in a mysterious spirit world when her parents take a wrong turn on the way to their new home. To find a way back, she must work in a magical bathhouse, make unexpected friends, and discover a courage she did not know she possessed.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Rumi Hiiragi", Role = "Chihiro" },
                        new CastMember { Name = "Miyu Irino", Role = "Haku" },
                        new CastMember { Name = "Mari Natsuki", Role = "Yubaba / Zeniba" },
                        new CastMember { Name = "Bunta Sugawara", Role = "Kamaji" }
                    })
                },
                new Movie
                {
                    Title = "Grave of the Fireflies",
                    Genre = "Animation",
                    ReleaseYear = 1988,
                    Rating = "8.5 / 10",
                    Director = "Isao Takahata",
                    Price = 12.99m,
                    PosterUrl = "https://upload.wikimedia.org/wikipedia/en/a/a5/Grave_of_the_Fireflies_Japanese_poster.jpg",
                    Description = "During the final months of World War II, siblings Seita and Setsuko struggle to survive after their home and family are torn apart. Their search for shelter and small moments of comfort becomes a deeply human story about resilience, love, and the cost of war.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Tsutomu Tatsumi", Role = "Seita Yokokawa" },
                        new CastMember { Name = "Ayano Shiraishi", Role = "Setsuko Yokokawa" },
                        new CastMember { Name = "Akemi Yamaguchi", Role = "Mother" },
                        new CastMember { Name = "Yoshiko Shinohara", Role = "Aunt" }
                    })
                },
                new Movie
                {
                    Title = "The Conjuring: Last Rites",
                    Genre = "Horror",
                    ReleaseYear = 2025,
                    Rating = "N/A / 10",
                    Director = "Michael Chaves",
                    Price = 18.99m,
                    PosterUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRbLTUdWonTPdE9Ozb3EYsJVj0S6RVwRsu645C9GA_SrQ&s=10",
                    Description = "Ed and Lorraine Warren face another case that tests their faith, experience, and devotion to one another. Set within the world of the Conjuring investigations, this supernatural horror story returns to the couple at the center of the franchise for a final unsettling chapter.",
                    Cast = JsonSerializer.Serialize(new[]
                    {
                        new CastMember { Name = "Vera Farmiga", Role = "Lorraine Warren" },
                        new CastMember { Name = "Patrick Wilson", Role = "Ed Warren" },
                        new CastMember { Name = "Mia Tomlinson", Role = "Judy Warren" },
                        new CastMember { Name = "Ben Hardy", Role = "Tony Spera" }
                    })
                }
            };

            foreach (var movie in originalCatalog)
            {
                var existing = context.Movies.FirstOrDefault(m => m.Title == movie.Title);
                if (existing == null)
                {
                    context.Movies.Add(movie);
                }
                else
                {
                    // Update any missing fields if already exists
                    if (string.IsNullOrEmpty(existing.PosterUrl)) existing.PosterUrl = movie.PosterUrl;
                    if (!existing.ReleaseYear.HasValue) existing.ReleaseYear = movie.ReleaseYear;
                    if (string.IsNullOrEmpty(existing.Director)) existing.Director = movie.Director;
                    if (string.IsNullOrEmpty(existing.Rating)) existing.Rating = movie.Rating;
                    if (string.IsNullOrEmpty(existing.Description)) existing.Description = movie.Description;
                    if (string.IsNullOrEmpty(existing.Cast)) existing.Cast = movie.Cast;
                }
            }

            context.SaveChanges();
        }
    }
}
