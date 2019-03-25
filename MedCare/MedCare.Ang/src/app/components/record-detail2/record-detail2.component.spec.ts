import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecordDetail2Component } from './record-detail2.component';

describe('RecordDetail2Component', () => {
  let component: RecordDetail2Component;
  let fixture: ComponentFixture<RecordDetail2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecordDetail2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecordDetail2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
