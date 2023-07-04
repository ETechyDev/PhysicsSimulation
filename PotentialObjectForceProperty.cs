﻿using System.Collections;
using static System.Math;

namespace Physics
{
    public partial class PotentialObject
    {
        public CartesianForce NetForce = CartesianForce.Instantiate(0f, 0f);

        protected ArrayList AppliedForces = new ArrayList();



        public new void AddTranslationForce(IForce force)
        {
            // Force components
            double[] forceComponents = force.GetComponents();

            // Assign to applied forces
            CartesianForce cartesianForce = CartesianForce.Instantiate(forceComponents[0], forceComponents[1]);
            AppliedForces.Add(cartesianForce);

            // Calculate net force
            UpdateNetForce(forceComponents[0], forceComponents[1]);
        }

        public override void AddTranslationForce(AbsoluteForce absoluteForce, Angle angle, double deltaTime)
        {
            double radians = angle.Radians;

            // Force magnitude
            double forceMagnitude = absoluteForce.Value;

            // Force components

            double forceX = forceMagnitude;
            double forceY = forceMagnitude;

            // X Component 

            if (Cos(radians) > 0)
                forceX *= Cos(angle.Radians);
            else
                forceX *= -Cos(angle.Radians);

            // Y Component

            if (Sin(radians) > 0)
                forceY *= Sin(radians);
            else
                forceY *= -Sin(radians);

            // Assign to applied forces
            CartesianForce cartesianForce = CartesianForce.Instantiate(forceX, forceY);
            AppliedForces.Add(cartesianForce);

            // Calculate net force
            UpdateNetForce(forceX, forceY);
        }



        protected void ApplyExponentialTranslationForce(ref CartesianForce force, double deltaTime)
        {
            // Force components
            double forceX = force.XValue;
            double forceY = force.YValue;

            // Calculate acceleration
            CartesianVector acceleration = CalculateExponentialAcceleration(forceX, forceY, deltaTime);

            // Calculate velocity
            CartesianVector velocity = CalculateExponentialVelocity(deltaTime);

            // Calculate position
            CartesianVector position = CalculatePosition(deltaTime);

            // Calculate force contract
            double forceContractX = forceX / deltaTime;
            double forceContractY = forceY / deltaTime;
            force = CartesianForce.Instantiate(forceContractX, forceContractY);

            // Calculate net force
            double netForceX = forceContractX - forceX;
            double netForceY = forceContractY - forceY;
            UpdateNetForce(netForceX, netForceY);

            // Update object's position & acceleration & velocity
            Acceleration = acceleration;
            Velocity = velocity;
            LocalPosition = position;
        }
    }
}