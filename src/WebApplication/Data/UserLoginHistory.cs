using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Data
{
    public class UserLoginHistory
    {
        public int UserID { get; set; }
        public string UserLoginHistoryID { get; set; }

        public virtual User User { get; set; }
        
        public string IpAddress { get; set; }

        public DateTime LastActivity { get; set; }


        public DateTime TimeStamp { get; set; }

        public string Browser { get; set; }

        //public DateTime Duration { get; set; }






        /// <summary>
        /// Returns the datetime threshold for when a user is considered active
        /// </summary>
        public static DateTime ActiveThreshold
        {
            get
            {
                // Putting this as a static method on User_Activity lets us define in a single place
                // what the threshold is defined as.  If we want to change the threshold we only
                // have to change it in one place
                return DateTime.Now.AddMinutes(-15);
            }
        }


        /// <summary>
        /// Returns if the user is considered to be currently online
        /// </summary>
        public bool IsOnline
        {
            get
            {
                // User is defined as online if they have been active within
                // the threshold.

                return this.LastActivity > ActiveThreshold;
            }
        }


    }
}
