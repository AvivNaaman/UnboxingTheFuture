using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtidRegister.Areas.Admin.Models;
using AtidRegister.Configuration;
using AtidRegister.Models.ViewModels;
using AtidRegister.Services;
using AtidRegister.Services.Conference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AtidRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PeopleController : Controller
    {
        #region constants
        private const string defaultBase64profile = "image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAYAAABw4pVUAAAABmJLR0QA/wD/AP+gvaeTAAAK9klEQVR4nO2da3BU5RnH/885u5vdTcIuuZEQkAiY4KBIDVQRCAmRCpgLjkZQC6jFfLCd0amtZaRjQweKdGxty7RT21osVbGiDIKijMUIiaPWy4CDQVQgXAyX3MhtLwl7nn5AwoZsstndd897MpzfTD7k7DnP/9n973t/z1nAxMTExMTExMTExMREX0h2ApFSUVGhNjc33wBgMhFdA2AUMycBsIY6n5mfq66u3qVrkjEwLAz5zoSFAJYCuBXAyAguP1BQUHBDVVWVFp/sxGJoQ/Lz861ut/tBTdNWEdHYGELd/e67724RllgcUWQnMBBFRUU3uVyu/cz81xjNAIAnq6qqDPteg1FlJxCKuXPnriSiFwFkCAqZcezYscScnJz2nJycM/X19YatvgxVZVVUVKhNTU3PEdHyOMo0AnhZVdUN77zzztdx1IkKwxhSVVWl1NTUbGTmZTpJBpj5eZvN9viuXbtadNIMi2GqLFVV1zLzT3SUVIjoRmZeNm7cuPfr6+tP6qg9IIYoIcXFxYuYeSvk5eNj5pLq6urdkvR7kW5IcXFxKjPXQVwDHi2dRHTT7t2762QmIb0ryMzrIN8MAEhi5pfz8/NDjvj1QqohxcXFuQAekJnDZVzvdrsflpmAVEOYeSUAS7x1iACnbWi1MzM/WlFRIa2zE/cPYyAKCwvdABbHU2NcqoI5eSrmTbbAohB++rIXp9o43GU5LS0tpQC2xTO3gZBmiKIoSwA4RcYkAq7NUjAnz4I5eRakJ/ctFU8vtuOx//hxum3wgTozL4UkQ6T1soqKit4iovkiYgWXhNHuwWvhs+08lJLSUlBQkC5jhliKITNmzHA4HI5WAAnRXB+uJITjdJsWtqQw89Tq6ur90eQXC1KqLKfTOZWZIzIjVhOCyXQp+N3ihHCm5AO4Mgxh5mlDOU+kCZcTzhRFUXKEiUWAFEOIaCLzwHV4JG1CLGS6FDyzxB6yTWHmnLgJD4KsEpIT/H9wSSjItSBjhH5NW8YIGqj3lalbEkHIMiSF6NKH/viCBPxgsrQeODJdClYUWLFmhz/4sNAu+VCRMlInoj5vNkGeF70o/QulQ0Ia0qZOouru6oyUHKXP9pr0xTTEYJiGGAwphiRY2C5DNxJk5ai7IfxS3prV85rHO62G3RoFp42xel7zeH4pd7Xe2roawptzF4N41fQxPvpjWSPSk8Jf09zmQ+1nZ/DN8faodb8+1o7az86gpc0f9tzUROCZkrOYPsZHIDzJm/Puilo4CvQdATAevji/PD6lB3++V8Gq1wc+va2jGxtePAh/dwAAcOe8HEybnBaR5McHmrD1v/UAgN0fncKjSyfDlRR62fzqNAXr7gDS0RN0VPsxgFcjEo0BfasswoTgf1MTgd8vsWNiRugV08MnO3rNAIC6w60RSx48cukan/88jpwIXdImZqjYcJ89RKmliRGLxoDebUjD5QecNkL2yNBzV2NGOaEEDaGvykqOWHBs5qVPWFUVZI9KDHle9kiCI/S6+7cRi8aAvlUWYRMY04d6ekaKAw/ekYt9XzYjK92Jm6ekRyw5Z1ombFYFp5u8mDopBRkpEXaeGJsiFo0BXVcM+RWoCORuBlABAEgcA1hDf2Ol0dMJdPUWildw6Kt7qAq6dQl1X8JlBmFzXgXAdyA5ew4sSVl65zAoPZ4GdJ3YA9BWLDn0GhHCblMRidStpPx26RowVsnMoR+EX9P8Hb+SJS936oTpgFT90HwhU1yuIWTZI1W/P4xAYK/MBKQaQvO3noKEnR0DQ5/R7TtPy8xA/mwv0b9lp9AL8wuyU5BvCPwbAXTIzgJAOzTtX7KTkG4Izd/VAsJfZOcB0J+o5M3I52YEI90QAICf1kLnKYrLOAF7wnqJ+r0YwhAq394BcCWg7yDsOzRAe4iKtnRK0O6HIQwBAFrwxk6An5Kg/Bta8KZhHk4j/abPYJhBeLt0E4Af6iJItAm3bb9f7+mRwTBMCQEAIjA67PeD+O86yD2PhPYfGckMwGAl5CLMILxV+hjAT4FI8P1+HADTz2jhjj+IjSsGQxpyEX7hqvVIynkcCS4xAbvPAe1H19PSkyvFBBSPAXbVDkLX8ePoPAEkZgOuiYA18hVDAEBPB9D2NdDVABCfEJukWIxtCACAga6TF/4SUi6YY08DrGG2rPR0Ar7GCyb4DfNsmbAMA0OC8Ldc+nBVG2BJAlQ7oFgBMKCdBwLeC2ZoPYOGMirDy5BgAt1AYPh884eKobq9JqYhhsOwhkx86oPS9a3LZ4mOu751+azxv61dKDquKAw1Dpm84eM7WxuanjjX2Hq9t6PLOsIJreWmSkURNJgOsILUj57V2r1QnMlJPa6MlP3uUSlr6h6ZPsiGVn2R3qgXVrN9hMVbSqBHTh05MbOudl/va+0eKK+dzUdFxidCtLY05qPde6FW8HR0Wj0dndPGTrp6W3mN71MC/hZwJLywYxp5hIhFibQqq/QTdpbXele6LL5vCfQKgJlp2aOgWvp+R6qOlsGvxf698WsWrD5a1ueYarUgNXsUAM5n8LOK13d0Ua3n0cJqefevSDGkbK9nieL1fQPGOgApF49bE2zImtD3mckHPVlYe6wkZs019SU45Ol76/no8WNhtdmCD2Uw0zMui+9Q6V5/ecyiUaBrG1JW2zWaWHkOwIBPAepsbcN7m9+Epl3avUnE2DhpI5ZlfhCV7uYz38fSuhXQgt4uKQoKl9yO5JTB5slom417KrcUJDdGJRwFupWQshrffLCyD4OYAQBJI13IuS63zzFmQuWhZfjnqcg7Xf9omI0HvnygjxkAcPV1uWHMAABe1E2WfWV7vAURC0eJLoYsqvE8RuCdBAxp+/q1t0yFK63vDyB0axas+HI57qt7CA1+d9gY3/rduPeLSlQeWobuy9ogV9pIXHvL1KGmP5oU7F5U46sc6gWxEN8qi5nKa/3rAP5FpJd62jtR++ou+Dzefq8lKOdRmrofC9M+x5TEk0i3XVgOb+xOwv7OsdjZPAVvNE8J2RmwOx2YdddtcI4Ywv10/fnl67Mda6O5cKjE1ZDyWu9aMJ6I9npPRyc+3F6NztY2IfkkupNxc8lcJLqjnMYHwIQnts9yrBOSUAjiZkj5Xu8KEGJeiu32dmP/ex/g1OHYljGyJozFDYUzYHPYwp88OEzA8m2zHXHZcRkXQ8r2emYS0XsQOPA8e7wBX/3vc7ScboroupSsNOROn4KMq0aLSgUA/ARt5rbZiZ+KDArEwZDST9ipen37GLhGdGwA6Gg5h3OHDyJQfwBn2wI47evbFmTaO5HhVqGOux7uCZOQnBK+AxAlB22qPX/LLdS/kYsB4YYs2uvZwER6/soBVN85MBM0h6C196HC9PTrBfafiwwp1JCSPb5rVIXrYIA5Mp3osYAmvTbbfkRUQKHjEFXhKlw5ZgCAtQccdS8yFMJKSFmtL4+Yv4CBfiRGJ85rGnJ3zHEcFRFMWAkh8DJceWYAgEUhLBUVTFyVpeFuYbGGG4R7RIUSYkjp+13TQdD1mSAGY1LZnq7viQgkxBBi5VYRcYYzpFKxiDiCDKEbRcQZ1jDliwgjqA1hIckMZ+jCw/tFxImNBR/yCFuP75yIWMMctp23j9hSRDHdGhdzCbFq/gyYZgAA+RRf5M+PuozYqywtELfZu+EGQYv5s4jZEEVTdJ7RMy4MZWT4swYn5nmngKq1q5o6LH48Pu5YAkZ4IoWJiYmJiYmJiYmJiUlE/B/au0+zccdGcAAAAABJRU5ErkJggg==";
        #endregion
        private readonly AppConfig _config;
        private readonly IPeopleService _svc;

        public PeopleController(IOptions<AppConfig> config, IPeopleService svc)
        {
            _config = config.Value;
            _svc = svc;
        }
        [HttpGet]
        public async Task<IActionResult> Index() => View(await _svc.GetAsync());
        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = Int32.MaxValue;
                if (model.PersonImage != null)
                {
                    string img = await ImageService.GetJpegBase64String(model.PersonImage);
                    if (!String.IsNullOrEmpty(img))
                        id = await _svc.AddAsync(model.FullName, model.JobTitle, img);
                    else
                    {
                        ModelState.AddModelError("PersonImage", "Please upload " + string.Join(", ", _config.AllowedImageFileExtensions) + " files only.");
                        return View(model);
                    }
                }
                else
                {
                    id = await _svc.AddAsync(model.FullName, model.JobTitle, defaultBase64profile);
                }
                return RedirectToAction("Details", new { id });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id) => View(await _svc.FindByIdAsync(id));
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.RemoveAsync(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var person = await _svc.FindByIdAsync(id);
            var model = new EditPersonViewModel() { FullName = person.FullName, JobTitle = person.JobTitle, OldImageFile = person.ImageFile };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditPersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNewImage = model.PersonImage != null;
                var person = await _svc.FindByIdAsync(model.Id);
                person.FullName = model.FullName;
                person.JobTitle = model.JobTitle;
                if (isNewImage)
                    person.ImageFile = await ImageService.GetJpegBase64String(model.PersonImage);
                await _svc.UpdateAsync(person);
                return RedirectToAction("Details", new { id = person.Id });
            }
            return View(model);
        }
    }
}