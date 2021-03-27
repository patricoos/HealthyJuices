import { Component } from '@angular/core';

@Component({
  template: ``
})
export abstract class MapComponent {
  public zoomDefault = 10;
  public zoom: number = this.zoomDefault;
  public centerLatitudeDefault = 52.22;
  public centerLongitudeDefault = 21.01;
  public centerLatitude: number = this.centerLatitudeDefault;
  public centerLongitude: number = this.centerLongitudeDefault;

  private maxZoomLevel = 16;

  public constructor() { }

  getZoomLevel(minLat: number, maxLat: number, minLng: number, maxLng: number): number {
    let angle = maxLng - minLng;
    const angle2 = maxLat - minLat;
    let delta = 0;

    if (angle2 > angle) {
      angle = angle2;
      delta = 3;
    }
    if (angle < 0) {
      angle += 360;
    }

    const zoomfactor = Math.floor(Math.log(960 * 360 / angle / 256) / Math.LN2) - 2 - delta;
    return zoomfactor;
  }

  setMapCenter(locations: any[]): void {
    if (locations.length > 0) {
      let latMax = 0;
      let lngMax = 0;
      let latMin = 99;
      let lngMin = 99;

      locations.forEach(l => {
        if (l.latitude > latMax) { latMax = l.latitude; }
        if (l.longitude > lngMax) { lngMax = l.longitude; }
        if (l.latitude < latMin) { latMin = l.latitude; }
        if (l.longitude < lngMin) { lngMin = l.longitude; }
      });

      this.centerLatitude = (latMax + latMin) / 2;
      this.centerLongitude = (lngMax + lngMin) / 2;

      this.zoom = this.getZoomLevel(latMin, latMax, lngMin, lngMax) + 1;

      if (this.zoom > this.maxZoomLevel) {
        this.zoom = this.maxZoomLevel;
      }
    } else {
      this.centerLatitude = this.centerLatitudeDefault;
      this.centerLongitude = this.centerLongitudeDefault;
      this.zoom = this.zoomDefault;
    }
  }
}
