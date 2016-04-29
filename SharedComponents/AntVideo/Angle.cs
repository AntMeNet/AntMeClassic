namespace AntMe.SharedComponents.AntVideo {
    internal static class Angle {
        public static int Interpolate(int angle, int delta) {
            int output = (angle + delta)%360;
            if (output < 0) {
                output += 360;
            }
            return output;
        }

        public static int Delta(int oldAngle, int newAngle) {
            // Cases
            // old: 100, new: 90 -> -10
            // old: 100, new: 110 -> 10
            // old: 350, new: 10 -> 20
            // old: 10, new 350 -> -20

            int output = newAngle - oldAngle;
            //if (output < -180) {
            //    // output += 360;
            //}
            //else if (output > 180) {
            //    // output -= 360;
            //}
            return output;
        }
    }
}