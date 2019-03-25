using MedCare.Models.Database;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MedCare.Data
{
    public class DBInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            // Look for any massages
            if (context.Doctors.Any())
            {
                return;   // DB has been seeded
            }

            var doctors = new Doctor[]
            {
            new Doctor{IsAdmin=true, FirstName="Johan", LastName="Johansson", Email="johan@medcare.se", Username ="john_j", PhoneNumber="123456789", SpecialistIn="Immunologists ", Description="I treat immune system disorders such as asthma, eczema, food allergies, insect sting allergies, and some autoimmune diseases."},
            new Doctor{IsAdmin=false, FirstName="Lars", LastName="Larsson", Email="lars@medcare.se",Username ="lars_8", PhoneNumber="123456789", SpecialistIn="Cardiologists", Description="I've specialized on the heart and blood vessels, everything from heart failure, a heart attack, high blood pressure, or an irregular heartbeat."},
            new Doctor{IsAdmin=false, FirstName="David", LastName="Davidsson", Email="david@medcare.se", Username ="david87", PhoneNumber="123456787", SpecialistIn="Neurologists", Description="Specialists in the nervous system, which includes the brain, spinal cord, and nerves. They treat strokes, brain and spinal tumors, epilepsy, Parkinson's disease, and Alzheimer's disease."},
            new Doctor{IsAdmin=false, FirstName="Gabriel", LastName="Gabrielsson", Email="gabriel@medcare.se", Username ="gabriel@medcare.se", PhoneNumber="123456786", SpecialistIn="Endocrinologist", Description="I am a doctor who has special training and experience in treating people with diabetes."},
            new Doctor{IsAdmin=false, FirstName="Sara", LastName="Hamid", Email="sara9@medcare.se", Username ="sara9", PhoneNumber="123456785", SpecialistIn="Nephrologists", Description="I treat kidney diseases as well as high blood pressure and fluid and mineral imbalances linked to kidney disease."}
            };

            foreach (Doctor m in doctors)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("12345", out passwordHash, out passwordSalt);
                m.PasswordHash = passwordHash;
                m.PasswordSalt = passwordSalt;
                m.Username = m.Username.ToLower();
                context.Doctors.Add(m);
            }
            context.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
