import { Component, OnInit } from '@angular/core';
import { Movie } from '../models/movie';
import { Observable } from 'rxjs';
import { MovieService } from '../services/movie.service';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.scss']
})

export class MovieComponent implements OnInit {

  movie$: Observable<Movie>;
  movieId: number;

  constructor(private movieService: MovieService, private route: ActivatedRoute, private sanitizer: DomSanitizer) {
    const idParam = 'id';
    if (this.route.snapshot.params[idParam]) {
      this.movieId = this.route.snapshot.params[idParam];
    }
  }

  ngOnInit() {
    this.getInfo();
  }
  /**
   * Method retrieves info of specific movie, using MovieService to call API
   * */
  getInfo() {
    this.movie$ = this.movieService.getMovie(this.movieId);
  }
  /**
   * Auxiliar method to bypass Security (youtube iframe)
   * 
   * @param item: 
   */
  getEmbedUrl(movie) {
    // Exposes your application to XSS security risks! (dev use only)
    return this.sanitizer.bypassSecurityTrustResourceUrl('https://www.youtube.com/embed/'+ movie.Trailer);
  }

}
