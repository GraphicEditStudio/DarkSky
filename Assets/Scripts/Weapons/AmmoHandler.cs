using System;

namespace Weapons
{
    public class AmmoHandler
    {
        public event Action<int, int, int, int> OnUpdate;
        
        private int inClip;
        private int clipSize;
        private int ammo;
        private int ammoSize;

        public void Initialize(int Clip, int ClipSize, int Ammo, int AmmoSize)
        {
            inClip = Clip;
            clipSize = ClipSize;
            ammo = Ammo;
            ammoSize = AmmoSize;
            OnUpdate?.Invoke(clipSize, ammoSize, inClip, ammo);
        }

        public bool CanReload()
        {
            return ammo > 0;
        }

        public bool GrabAmmo()
        {
            var grabbed = false;
            if (inClip > 0)
            {
                grabbed = true;
                inClip--;
            }
            OnUpdate?.Invoke(clipSize, ammoSize, inClip, ammo);
            return grabbed;
        }

        public int AddAmmo(int Amount)
        {
            var amountLeft = 0;
            ammo += Amount;
            if (ammo > ammoSize)
            {
                amountLeft = ammo - ammoSize;
                ammo = ammoSize;
            }
            OnUpdate?.Invoke(clipSize, ammoSize, inClip, ammo);
            return amountLeft;
        }

        public bool NeedsReload()
        {
            return inClip < clipSize;
        }

        public bool ReloadAllowed()
        {
            return NeedsReload() && CanReload();
        }
        
        public void Reload()
        {
            if (inClip < clipSize)
            {
                var amountNeeded = clipSize - inClip;
                if (amountNeeded > ammo)
                {
                    inClip += ammo;
                    ammo = 0;
                }
                else
                {
                    inClip = clipSize;
                    ammo -= amountNeeded;
                }
            }

            OnUpdate?.Invoke(clipSize, ammoSize, inClip, ammo);
        }

        public void ManualUpdate()
        {
            OnUpdate?.Invoke(clipSize, ammoSize, inClip, ammo); 
        }
    }
}