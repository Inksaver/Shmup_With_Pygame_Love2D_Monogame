import pygame
import shared

class Explosion():
	def __init__(self, images, center, key):
		self.key = key
		self.images = images
		self.image = images[self.key][0] #set to first image in the sequence
		self.rect = self.image.get_rect()
		self.rect.center = center
		self.frame = 0
		self.time_passed = 0
		self.frame_rate = 0.1 
		self.active = True

	def update(self, dt):
		self.time_passed += dt
		if self.time_passed >= self.frame_rate:
			self.time_passed = 0
			self.frame += 1
			if self.frame >= len(self.images[self.key]):
				self.active = False
			else:
				# ensure image sequence is centered per frame
				center = self.rect.center 
				self.image = self.images[self.key][self.frame] #next frame
				self.rect = self.image.get_rect()
				self.rect.center = center
				
		return self.active
	
	def draw(self):
		shared.screen.blit(self.image, self.rect)