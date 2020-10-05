using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LaserEmitter : BasicInput {

    const float defaultIndex = 1f;

    public int maxReflect = 10;
    public int maxDist = 50;
    public GameObject emitter;

    int points = 0;
    Collider within;
    float currentIndex;

    LineRenderer line;

    // Use this for initialization
    new void Start() {
        base.Start();
        line = emitter.GetComponent<LineRenderer>();
        line.startWidth = .1f;
        line.endWidth = .1f;

        within = null;
        currentIndex = defaultIndex;
    }

    public override void Activate() {
        if (input.state) {
            line.positionCount = 1;
            line.SetPosition(0, emitter.transform.position);

            //ps.Play();
            points = 0;
            Ray ray = new Ray(emitter.transform.position, emitter.transform.forward);
            within = null;
            currentIndex = defaultIndex;
            ShootLaser(ray);
        }

        line.enabled = input.state;
    }

    public override void Deactivate() {
        line.enabled = false;
    }

    void ShootLaser(Ray inRay, bool reverse = false) {
        // reverse the ray for a reverse-raycast
        Ray ray = reverse ? new Ray(inRay.GetPoint(maxDist), -inRay.direction) : inRay;

        // collision in general and specifically for the refractor object changes for reverse-raycasts
        RaycastHit rayHit;
        bool hasHit = reverse ? within.Raycast(ray, out rayHit, maxDist) : Physics.Raycast(ray, out rayHit, maxDist);

        if (hasHit) {
            if (rayHit.transform.GetComponent<Reflector>() != null) { // encounters reflection
                AddPoint(rayHit.point);

                if (points < maxReflect) {
                    Ray reflected = new Ray(rayHit.point, Vector3.Reflect(rayHit.point - line.GetPosition(line.positionCount - 2), rayHit.normal));
                    ShootLaser(reflected);
                }
            } else if (rayHit.transform.GetComponent<Refractor>() != null) { // encounters refraction
                int inverse = reverse ? -1 : 1;
                float enteringIndex = within == null ? rayHit.transform.GetComponent<Refractor>().index : defaultIndex;

                AddPoint(rayHit.point);

                if (points < maxReflect) {
                    // calculate angle between the ray and the collision normal
                    float angleIncidence = Vector3.Angle(-ray.direction.normalized, rayHit.normal.normalized * inverse) * Mathf.Deg2Rad;
                    // calculate refraction angle using Snell's Law
                    float angleRefracted = Mathf.Asin(currentIndex * Mathf.Sin(angleIncidence) / enteringIndex);

                    //Debug.Log(angleIncidence * Mathf.Rad2Deg + ":" + angleRefracted * Mathf.Rad2Deg);

                    // if the float is NaN, the refraction is invalid and means that there is total internal reflection
                    if (!float.IsNaN(angleRefracted)) { // normal refraction
                        // direction of the refracted ray
                        Vector3 cross = Vector3.Cross(rayHit.normal.normalized, -ray.direction.normalized);
                        Vector3 refractedDir = Quaternion.AngleAxis(angleRefracted * Mathf.Rad2Deg, cross) * (-rayHit.normal * inverse);
                        // shift new starting point slightly so it crosses collider wall
                        Ray refracted = new Ray(rayHit.point + refractedDir.normalized * Mathf.Epsilon, refractedDir);
                        currentIndex = enteringIndex;

                        // if exiting an object, clear the collider
                        if (within == null)
                            within = rayHit.collider;
                        else within = null;

                        ShootLaser(refracted, within != null);

                    } else { // total internal reflection
                        Ray reflected = new Ray(rayHit.point, Vector3.Reflect(rayHit.point - line.GetPosition(line.positionCount - 2), rayHit.normal));
                        ShootLaser(reflected, true);
                    }
                }
            } else { // encounters regular object, terminates by absorption
                if (rayHit.transform.GetComponent<LaserReceiver>() != null)
                    rayHit.transform.GetComponent<LaserReceiver>().BeamHit();

                AddPoint(rayHit.point);
                //ps.transform.position = hit.point;
                //ps.transform.LookAt(transform.position);
                //psr.material = hit.transform.GetComponent<Renderer>().material;
            }

        } else { // encounters nothing, terminates at maximum distance
            //ps.Stop();
            AddPoint(ray.GetPoint(maxDist));
        }
    }

    private void AddPoint(Vector3 point) {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, point);
    }

}
