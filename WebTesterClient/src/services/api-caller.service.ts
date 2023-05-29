import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, combineLatest, defer, of, switchMap } from 'rxjs';
import { FileSize } from 'src/app/types/FileSize';
import { ChunkSize } from 'src/app/types/ChunkSize';

@Injectable({
  providedIn: 'root'
})
export class ApiCallerService {
  constructor(private http: HttpClient) { }

  retrieveFileFromEndpoint(url: string, queryParams: any): Observable<number[]> {
      return defer(() => {
      let apiEndpoint = `${url}\\download-data`
      apiEndpoint = `${apiEndpoint}?fileSizeId=${queryParams.fileSizeId}`;
      apiEndpoint = `${apiEndpoint}&chunkSizeId=${queryParams.chunkSizeId}`;
      const options = { responseType: 'blob' as 'json' };
      const startTime = performance.now();
      return combineLatest([this.http.get<any>(apiEndpoint, options), of(performance.now())])
        .pipe(switchMap(() => of([startTime, performance.now()])));
    })
  }

  getFileSizes(url: string): Observable<FileSize[]>{
    return this.http.get<any>(`${url}/file-sizes`);
  }

  getChunkSizes(url: string): Observable<ChunkSize[]>{
    return this.http.get<any>(`${url}/chunk-sizes`)
  }
}
