using System;

namespace MAGLabCL
{
    public class Timeout
    {
        public UInt16 lastRecTime = 0;
        public UInt16 timeout = 600;

        public Timeout()
        {
            lastRecTime = 0;
            timeout = 600;
        }

        public Timeout(UInt16 n_Timeout)
        {
            lastRecTime = 0;
            timeout = n_Timeout;
        }

        public bool update(bool received, UInt16 time)
        {
            bool retVal = false;
            if (received)
            {
                lastRecTime = time;
                retVal = false;
            }

            if (time - lastRecTime >= timeout)
            {
                retVal = true;
            }

            if (time - lastRecTime > timeout)
            {
                lastRecTime += 1;
            }

            return retVal;
        }
    }

    public class ConfirmationThreshold<T> where T : IComparable
    {
        public T confirmedValue;
        public UInt16 lastConfTime = 0;
        public UInt16 confNumCounter = 0;
        public UInt16 tReq;
        public UInt16 nReq;
        public ConfirmationThreshold(T initValue, UInt16 time_required, UInt16 number_required)
        {
            confirmedValue = initValue;
            tReq = time_required;
            nReq = number_required;
        }

        public ConfirmationThreshold(T initValue)
        {
            confirmedValue = initValue;
            tReq = 900;
            nReq = 3; 
        }

        public T update(bool received, T newValue, UInt16 time)
        {
            // confirmation state machine
            if (received)
            {
                // received counter processing
                if (confNumCounter <= nReq)
                {
                    confNumCounter += 1;
                }

                // new value processing
                if (confirmedValue.CompareTo(newValue) != 0)
                {
                    // if both the numeric value and the counter are above the ratio, we have a new value
                    if (confNumCounter >= nReq)
                    {
                        if (time - lastConfTime > tReq)
                        {
                            confirmedValue = newValue;
                        }
                    }
                }

                // confirmed value processing
                if (confirmedValue.CompareTo(newValue) == 0)
                {
                    confNumCounter = 0;
                    lastConfTime = time;
                }
            }

            if (time - lastConfTime > tReq)
            {
                lastConfTime += 1;
            }

            return confirmedValue;
        }
    }
}
