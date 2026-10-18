using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MovieStore.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Genre { get; set; } = "";

        public string Description { get; set; } = "";

        [Range(0, 1000)]
        public decimal Price { get; set; }

        /// <summary>URL to the movie poster image (external or relative path).</summary>
        public string? PosterUrl { get; set; }

        /// <summary>Year the movie was released.</summary>
        public int? ReleaseYear { get; set; }

        /// <summary>Director of the movie.</summary>
        public string? Director { get; set; }

        /// <summary>Rating of the movie (e.g. "8.5 / 10").</summary>
        public string? Rating { get; set; }

        /// <summary>JSON serialized cast members list.</summary>
        public string? Cast { get; set; }

        [NotMapped]
        public List<CastMember> CastMembers
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Cast)) return new List<CastMember>();
                try
                {
                    return JsonSerializer.Deserialize<List<CastMember>>(Cast, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CastMember>();
                }
                catch
                {
                    return new List<CastMember>();
                }
            }
        }
    }

    public class CastMember
    {
        public string Name { get; set; } = "";
        public string Role { get; set; } = "";

        private static readonly HashSet<string> AvailableCastImages = new(StringComparer.OrdinalIgnoreCase)
        {
            "adam-driver.jpg", "al-pacino.jpg", "ana-de-armas.jpg", "anna-paquin.jpg",
            "billy-crystal.jpg", "candice-bergen.jpg", "carrie-fisher.jpg", "chris-hemsworth.jpg",
            "daisy-edgar-jones.jpg", "dwayne-johnson.jpg", "enzo-vogrincic.jpg", "gal-gadot.jpg",
            "golshifteh-farahani.jpg", "jacob-batalon.jpg", "jamie-foxx.jpg", "jennifer-garner.jpg",
            "jennifer-lawrence.jpg", "joe-pesci.jpg", "john-boyega.jpg", "jonah-hill.jpg",
            "josh-lucas.jpg", "kiefer-sutherland.jpg", "laura-dern.jpg", "leonardo-dicaprio.jpg",
            "mari-natsuki.jpg", "mark-ruffalo.jpg", "meg-ryan.jpg", "meryl-streep.jpg",
            "patrick-dempsey.jpg", "patrick-wilson.jpg", "randeep-hooda.jpg", "ray-liotta.jpg",
            "reese-witherspoon.jpg", "rege-jean-page.jpg", "robert-de-niro.jpg", "rudhraksh-jaiswal.jpg",
            "ryan-gosling.jpg", "ryan-reynolds.jpg", "sadie-sink.jpg", "scarlett-johansson.jpg",
            "teyonah-parris.jpg", "tom-holland.jpg", "vera-farmiga.jpg", "walker-scobell.jpg",
            "zendaya.jpg"
        };

        public string GetNormalizedImageName()
        {
            if (string.IsNullOrWhiteSpace(Name)) return "";
            var normalized = Name.Normalize(System.Text.NormalizationForm.FormD);
            var clean = Regex.Replace(normalized, @"[\u0300-\u036f]", "");
            clean = Regex.Replace(clean.ToLowerInvariant(), @"[^a-z0-9]+", "-").Trim('-');
            return clean + ".jpg";
        }

        public string GetPhotoUrl()
        {
            var fileName = GetNormalizedImageName();
            if (AvailableCastImages.Contains(fileName))
            {
                return $"/imagesaces/{fileName}";
            }
            return "/images/favicon.png";
        }
    }
}