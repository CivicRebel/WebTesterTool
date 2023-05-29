import { ApiCallerService } from './../services/api-caller.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FileSize } from './types/FileSize';
import { Observable, OperatorFunction, catchError, combineLatest, concat, concatMap, debounceTime, of, pipe, repeat, switchMap, tap } from 'rxjs';
import { ChunkSize } from './types/ChunkSize';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  public callResults: any[] = [];
  public fileSizes: FileSize[] = [];
  public chunkSizes: ChunkSize[] = [];
  public meanOutputRate = null;
  public meanLatency = null;
  public benchmarkInProgress = false;
  public benchmarkConfiguration: FormGroup = new FormGroup({});

  constructor(private service: ApiCallerService, private fb: FormBuilder){
  }

  ngOnInit(){
    this.benchmarkConfiguration = this.fb.group({
      fileSizeId: new FormControl<number | null>(null, [Validators.required]),
      chunkSizeId: new FormControl<number | null>(null, [Validators.required]),
      simultaneousCalls: new FormControl<boolean>(false, [Validators.required]),
      apiCallRepeat: new FormControl<number>(1, [Validators.required, Validators.min(1)]),
      endpoint: new FormControl<string>("")
    })
    
    this.benchmarkConfiguration.controls['endpoint'].valueChanges.pipe(
      tap(() => {this.chunkSizes = []; this.fileSizes = []; this.meanLatency = null; this.meanOutputRate = null;}),
      debounceTime(500),
      switchMap((value) => combineLatest([this.service.getFileSizes(value), this.service.getChunkSizes(value)])
      .pipe(
        catchError(() => {
          console.log('Cannot connect to endpoint');
          return of([]);
        })
      ))
    ).subscribe(([fileSizes, chunkSizes]) => {
      this.fileSizes = fileSizes;
      this.chunkSizes = chunkSizes; 
    });
  }

  runBenchmark(){
    if(!this.benchmarkConfiguration.valid){ 
      return;
    }

    this.resetBenchmarkResults();
    const configurationValue = this.benchmarkConfiguration.getRawValue();
    const fileSize = this.fileSizes.find(x => x.id == configurationValue.fileSizeId)?.sizeInMb ?? 0;

    this.benchmarkInProgress = true;
    const operation = this.getOperation(configurationValue);

    for(var i = 0; i < (configurationValue.simultaneousCalls ? configurationValue.apiCallRepeat : 1 ); i++){
      operation
      .subscribe((value) => 
      this.onFileDownloaded(value, configurationValue.apiCallRepeat, fileSize));
    }
  }

  private onFileDownloaded(timestamps: number[], apiCallRepeat: number, fileSize: number){
      const latencyInSec = (timestamps[1] - timestamps[0])/1000;
      this.callResults.push({latency: latencyInSec, outputRate: (fileSize / latencyInSec).toFixed(2)})
      if(this.callResults.length == apiCallRepeat){
        this.computeMeans();
        this.benchmarkInProgress = false;
      }
  }

  private resetBenchmarkResults(): void{
    this.callResults = [];
    this.meanOutputRate = null;
    this.meanLatency = null;
  }

  private getOperation(configurationValue: any): Observable<number[]>{
    const apiCall = this.service.retrieveFileFromEndpoint(configurationValue.endpoint,
      {
        fileSizeId: configurationValue.fileSizeId,
        chunkSizeId: configurationValue.chunkSizeId
      });
      
    const operation = configurationValue.simultaneousCalls ? 
    apiCall : of(null).pipe(repeat(configurationValue.apiCallRepeat),
    concatMap(() => apiCall)); 

    return operation;
  }

  private computeMeans(){
    this.meanOutputRate = this.callResults.reduce((acc, currentValue) => 
      acc + currentValue.outputRate / this.callResults.length, 0).toFixed(2);
    this.meanLatency = this.callResults.reduce((acc, currentValue) => 
    acc + currentValue.latency / this.callResults.length, 0).toFixed(2);
  }
}
